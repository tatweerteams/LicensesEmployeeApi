using Infra;
using Microsoft.EntityFrameworkCore;
using SendEventBus.PublishEvents;
using TrackingServices.Domain;

namespace Services
{
    public interface ITrackServices
    {
        Task GetOrderRequestIsPrinting();
        Task GetOrderRequestIsDone();

    }
    public class TrackServices : ITrackServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OrderRequestChengeStauesPublish _chengeStauesPublish;

        public TrackServices(IUnitOfWork unitOfWork, OrderRequestChengeStauesPublish chengeStauesPublish)
        {
            _unitOfWork = unitOfWork;
            _chengeStauesPublish = chengeStauesPublish;
        }
        public async Task GetOrderRequestIsDone()
        {

            var result = await (await _unitOfWork.GetRepositoryReadOnly<UserPrinting>().
                 FindBy(pred => pred.IsDone.Equals(true) && pred.IsSendQueue.Equals(false))).Include(i => i.Files).ToListAsync();

            if (result.Any())
            {
                var c = result.All(a =>
                {
                    a.IsSendQueue = true;
                    return true;
                });

                var trans = _unitOfWork.BignTransation();
                try
                {
                    await _unitOfWork.SaveChangeAsync();
                    var identityNumber = result.Select(s => s.Files.FileNumber.ToString()).ToList();

                    await _chengeStauesPublish.
                        ChangeOrderRequestStatus(identityNumber, OrderRequestState.OrderRequestPrintedDone);
                    trans.Commit();

                }
                catch (Exception)
                {
                    trans.Rollback();

                }
                finally { trans.Dispose(); }




            }

        }

        public async Task GetOrderRequestIsPrinting()
        {
            var result = await (await _unitOfWork.GetRepositoryReadOnly<UserPrinting>().
                FindBy(pred => pred.IsDownloaded.Equals(true) && pred.IsSendQueue == null)).Include(i => i.Files).ToListAsync();

            if (result.Any())
            {
                result.All(a =>
                {
                    a.IsSendQueue = false;
                    return true;
                });

                var trans = _unitOfWork.BignTransation();
                try
                {
                    await _unitOfWork.GetRepositoryWriteOnly<UserPrinting>().UpdateAll(result);
                    await _unitOfWork.SaveChangeAsync();
                    var identityNumber = result.Select(s => s.Files.FileNumber.ToString()).ToList();

                    await _chengeStauesPublish.
                        ChangeOrderRequestStatus(identityNumber, OrderRequestState.OrderRequestPrinting);
                    trans.Commit();

                }
                catch (Exception)
                {
                    trans.Rollback();

                }
                finally { trans.Dispose(); }







            }


        }
    }
}
