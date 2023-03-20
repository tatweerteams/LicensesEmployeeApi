using AutoMapper;
using FilterAttributeWebAPI.Common;
using Infra;
using Infra.Utili;
using Newtonsoft.Json;
using SendEventBus.PublishEvents;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.Events;
using SharedTatweerSendData.Models.RegionModel;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.services
{
    public interface IRegionServices
    {
        Task AddRegion(InsertRegionModel region, CancellationToken cancellationToken = default);
        Task UpdateRegion(UpdateRegionModel region, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<RegionDTO>> GetRegions(string regionName, string regionNo, CancellationToken cancellationToken = default);
        Task DeleteRegion(string id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<RegionDTO>> GetRegionUnSelectedBank(string bankId, CancellationToken cancellationToken = default);

    }

    public class RegionServices : IRegionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly LogginDataPublish _logginDataPublish;
        private readonly HelperUtili _helper;
        public RegionServices(IUnitOfWork unitOfWork, IMapper mapper, LogginDataPublish logginDataPublish, HelperUtili helper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logginDataPublish = logginDataPublish;
            _helper = helper;
        }
        public async Task AddRegion(InsertRegionModel region, CancellationToken cancellationToken = default)
        {

            var currentUser = _helper.GetCurrentUser();
            await _unitOfWork.GetRepositoryWriteOnly<Region>().Insert(_mapper.Map<Region>(region), cancellationToken);

            await _unitOfWork.SaveChangeAsync();


            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Insert,
                Messages = "تم ‘ضافة منطقة جديدة",
                NewData = JsonConvert.SerializeObject(region),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }



        public async Task DeleteRegion(string id, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<Region>().GetByID(id);

            if (result == null)
                throw new ApplicationEx("بيانات المنطقة غير موجودة");


            await _unitOfWork.GetRepositoryWriteOnly<Region>().Remove(result);

            await _unitOfWork.SaveChangeAsync();

            var currentUser = _helper.GetCurrentUser();
            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Delete,
                Messages = $"تم حذف منطقة تحت رقم تعريف {id}",
                NewData = JsonConvert.SerializeObject(result),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }

        public async Task<IReadOnlyList<RegionDTO>> GetRegions(string regionName, string regionNo, CancellationToken cancellationToken = default)
        => (await _unitOfWork.GetRepositoryReadOnly<Region>().
                FindBy(
                     predicate: pred =>
                                (string.IsNullOrWhiteSpace(regionName) || pred.Name.Contains(regionName.Trim())) &&
                                (string.IsNullOrWhiteSpace(regionNo) || pred.RegionNo.Contains(regionNo.Trim())),

                     selector: select => new RegionDTO
                     {
                         CreateRegionDate = select.CreateAt.ToShortDateString(),
                         RegionId = select.Id,
                         RegionName = select.Name,
                         RegionNumber = select.RegionNo,
                         CreateAt = select.CreateAt,
                     }
                )).OrderByDescending(order => order.CreateAt).ToList();

        public async Task<IReadOnlyList<RegionDTO>> GetRegionUnSelectedBank(string bankId, CancellationToken cancellationToken = default)
        => (await _unitOfWork.GetRepositoryReadOnly<Region>().FindBy(

                    predicate: pred => !pred.BankRegions.Any(bank => bank.BankId.Equals(bankId)),

                    selector: select => new RegionDTO
                    {
                        RegionId = select.Id,
                        RegionName = select.Name,
                        RegionNumber = select.RegionNo
                    })).OrderBy(order => order.RegionName).ToList();

        public async Task UpdateRegion(UpdateRegionModel region, CancellationToken cancellationToken = default)
        {
            var oldData = await _unitOfWork.GetRepositoryReadOnly<Region>().GetByID(region.RegionId);

            if (oldData == null)
                throw new ApplicationEx("بيانات المنطقة غير موجودة");

            var currentUser = _helper.GetCurrentUser();
            var @event = new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Update,
                Messages = $"تم تعديل منطقة تحت رقم تعريف {region.RegionId}",
                NewData = JsonConvert.SerializeObject(region),
                OldData = JsonConvert.SerializeObject(oldData),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            };
            await _unitOfWork.GetRepositoryWriteOnly<Region>().Update(_mapper.Map(region, oldData));
            await _unitOfWork.SaveChangeAsync();


            await _logginDataPublish.PublishEventData(@event);

        }
    }
}
