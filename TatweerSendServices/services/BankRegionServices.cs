using AutoMapper;
using FilterAttributeWebAPI.Common;
using Infra;
using Infra.Utili;
using Newtonsoft.Json;
using SendEventBus.PublishEvents;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.Events;
using SharedTatweerSendData.Models;
using TatweerSendDomain.Domain;
using TatweerSendServices.ExtensionServices;

namespace TatweerSendServices.services
{
    public interface IBankRegionServices
    {
        Task DeleteBankRegion(string bankRegionId, CancellationToken cancellationToken = default);
        Task Activation(string bankRegionId, bool isActive, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<BankRegionActiveDTO>> GetActive(string bankId, string regionName, string regionNo, CancellationToken cancellationToken = default);
        Task<PaginationDto<BankRegionDTO>> GetAll(string bankId, string regionName, string regionNo, int pageNo = 1, int pageSize = 30, CancellationToken cancellationToken = default);
        Task AddBankRegion(InsertBankRegionModel bankRegion, CancellationToken cancellationToken = default);


    }

    public class BankRegionServices : IBankRegionServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly LogginDataPublish _logginDataPublish;
        private readonly HelperUtili _helper;
        public BankRegionServices(IUnitOfWork unitOfWork, IMapper mapper, LogginDataPublish logginDataPublish, HelperUtili helper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logginDataPublish = logginDataPublish;
            _helper = helper;
        }
        public async Task Activation(string bankRegionId, bool isActive, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<BankRegion>().GetByID(bankRegionId);

            if (result == null)
                throw new ApplicationEx("بيانات المنطقة المصرف غير موجودة");

            result.IsActive = !isActive;

            await _unitOfWork.SaveChangeAsync();

            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Activation,
                Messages = "تم تغيير حالة المنطقة",
                OldData = isActive.ToString(),
                NewData = result.ToString(),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }

        public async Task AddBankRegion(InsertBankRegionModel bankRegion, CancellationToken cancellationToken = default)
        {

            await _unitOfWork.GetRepositoryWriteOnly<BankRegion>().Insert(_mapper.Map<BankRegion>(bankRegion));
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Insert,
                Messages = "تم إضافة  منطقة للمصرف",
                NewData = JsonConvert.SerializeObject(bankRegion),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });

        }

        public async Task DeleteBankRegion(string bankRegionId, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<BankRegion>().GetByID(bankRegionId);

            if (result == null)
                throw new ApplicationEx("بيانات المنطقة المصرف غير موجودة");

            await _unitOfWork.GetRepositoryWriteOnly<BankRegion>().Remove(result);

            await _unitOfWork.SaveChangeAsync();
            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Delete,
                Messages = "تم إلغاء  منطقة للمصرف",
                OldData = JsonConvert.SerializeObject(result),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }

        public async Task<IReadOnlyList<BankRegionActiveDTO>> GetActive(string bankId, string regionName, string regionNo, CancellationToken cancellationToken = default)
        => await _unitOfWork.GetRepositoryReadOnly<BankRegion>().FindBy(
                   predicate: bankId.SearchBankRegionActiveExpression(regionName),
                   selector: select => new BankRegionActiveDTO
                   {

                       BankRegionId = select.Id,
                       RegionName = select.Region.Name,
                       RegionNumber = select.Region.RegionNo,
                   });

        public async Task<PaginationDto<BankRegionDTO>> GetAll(string bankId, string regionName, string regionNo, int pageNo = 1, int pageSize = 30, CancellationToken cancellationToken = default)
        {


            var filterResult = (await _unitOfWork.GetRepositoryReadOnly<BankRegion>().
                FindBy(
                    predicate: bankId.SearchBankRegionExpression(regionName),
                    selector: select => new BankRegionDTO
                    {
                        BankRegionId = select.Id,
                        IsActive = select.IsActive,
                        RegionName = select.Region.Name,
                        RegionNumber = select.Region.RegionNo,
                        BankId = select.BankId,
                        BankName = select.Bank.Name,
                        BankNo = select.Bank.BankNo,
                        BranchCount = select.Branchs.Count(),
                        CreateAt = select.CreateAt,
                    },
                    pageNo: pageNo,
                    pageSize: pageSize
                    )).OrderBy(order => order.BankName).ThenBy(d => d.RegionNumber).ToList();


            var totalRecordCount = await _unitOfWork.GetRepositoryReadOnly<BankRegion>().GetCount(bankId.SearchBankRegionExpression(regionName));

            return new PaginationDto<BankRegionDTO>()
            {
                Data = filterResult,
                PageCount = totalRecordCount > 0
             ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
             : 0
            };

        }
    }
}
