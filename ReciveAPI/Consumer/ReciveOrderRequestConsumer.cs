using Events;
using Infra;
using MassTransit;
using ReciveAPI.Domain;

namespace ReciveAPI.Consumer
{
    public class ReciveOrderRequestConsumer : IConsumer<ReciveOrderRequestEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReciveOrderRequestConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Consume(ConsumeContext<ReciveOrderRequestEvent> context)
        {
            try
            {
                var listItem = new List<Table>();
                int index = 0;
                context.Message.OrderItems.ForEach(item =>
                {
                    index++;
                    listItem.Add(new Table
                    {
                        AccountName = item.AccountName,
                        AccountNumber = item.AccountNumber,
                        BranchName = item.BranchName,
                        BranchNumber = item.BranchNumber,
                        ChCount = item.ChCount,
                        ForCount = item.ForCount,
                        FromSerial = item.FromSerial,
                        MyUser = item.MyUser,
                        RegionNumber = item.RegionNumber,
                        RegName = item.RegName,
                        RequestDate = item.RequestDate,
                        RequestIdentity = item.RequestIdentity,
                        RequestStatus = 1,
                        IsDone = null,
                        Tc = item.Tc,
                        Increment = index
                    });

                });

                await _unitOfWork.GetRepositoryWriteOnly<Table>().InsertList(listItem);
                await _unitOfWork.SaveChangeAsync();


            }
            catch (Exception ex)
            {

                throw;
            }


        }


    }
}
