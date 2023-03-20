using AutoMapper;
using FilterAttributeWebAPI.Common;
using Infra;
using Infra.Utili;
using MassTransit;
using Newtonsoft.Json;
using SendEventBus.PublishEvents;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.Events;
using SharedTatweerSendData.Models;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.services
{
    public interface IBankServices
    {
        Task InsertBank(InsertBankModel bankModel, CancellationToken cancellationToken = default);
        Task UpdateBank(UpdateBankModel bankModel, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<BankDTO>> GetBanks(string bankName, string bankNo, string regionId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ActiveBankDTO>> GetActiveBanks(string bankName, string bankNo, CancellationToken cancellationToken = default);
        Task ActivationBank(string bankId, bool isActive, CancellationToken cancellationToken = default);
        Task DeleteBank(string bankId, CancellationToken cancellationToken = default);

    }

    public class BankServices : IBankServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly LogginDataPublish _logginDataPublish;
        private readonly HelperUtili _helper;
        public BankServices(IUnitOfWork unitOfWork, IMapper mapper, LogginDataPublish logginDataPublish, HelperUtili helper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logginDataPublish = logginDataPublish;
            _helper = helper;
        }
        public async Task ActivationBank(string bankId, bool isActive, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<Bank>().GetByID(bankId);

            if (result == null)
                throw new ApplicationEx("بيانات المصرف غير موجودة");

            result.IsActive = !isActive;
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Activation,
                Messages = "تم تغيير حالة المصرف",
                OldData = $"المصرف :{result.Name} الحالة : {isActive}",
                NewData = $"المصرف :{result.Name} الحالة : {result.IsActive}",
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }

        public async Task DeleteBank(string bankId, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<Bank>().GetByID(bankId);

            if (result == null)
                throw new ApplicationEx("بيانات المصرف غير موجودة");

            await _unitOfWork.GetRepositoryWriteOnly<Bank>().Remove(result);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Delete,
                Messages = "تم حذف المصرف",
                OldData = result.Name,
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }

        public async Task<IReadOnlyList<ActiveBankDTO>> GetActiveBanks(string bankName, string bankNo, CancellationToken cancellationToken = default)
        => (await _unitOfWork.GetRepositoryReadOnly<Bank>().FindBy(
                    predicate: pred =>
                        pred.IsActive == true &&
                       (string.IsNullOrWhiteSpace(bankName) || pred.Name.Contains(bankName)) &&
                       (string.IsNullOrWhiteSpace(bankNo) || pred.BankNo.Contains(bankNo)),
                    selector: select => new ActiveBankDTO
                    {
                        Name = select.Name,
                        BankNo = select.BankNo,
                        BankId = select.Id
                    })).OrderBy(order => order.Name).ToList();


        public async Task<IReadOnlyList<BankDTO>> GetBanks(string bankName, string bankNo, string regionId, CancellationToken cancellationToken = default)
            => (await _unitOfWork.GetRepositoryReadOnly<Bank>().FindBy(
                predicate: pred =>
                    (string.IsNullOrWhiteSpace(bankName) || pred.Name.Contains(bankName.Trim())) &&
                    (string.IsNullOrWhiteSpace(bankNo) || pred.BankNo.Contains(bankNo.Trim())),
                selector: select => new BankDTO
                {
                    BankId = select.Id,
                    BankNo = select.BankNo,
                    IsActive = select.IsActive,
                    Name = select.Name,
                    BankRegionCount = select.BankRegions.Count(),
                    CreateAt = select.CreateAt,
                    BankRegions = select.BankRegions.Select(region => new BankRegionDTO
                    {
                        BankRegionId = region.Id,
                        IsActive = region.IsActive,
                        RegionName = region.Region.Name,
                        RegionNumber = region.Region.RegionNo,

                    }).ToList()

                })).OrderByDescending(order => order.CreateAt).ToList();



        public async Task InsertBank(InsertBankModel bankModel, CancellationToken cancellationToken = default)
        {

            bankModel.BankRegions.All(c =>
            {
                c.UserId = bankModel.UserId;
                return true;
            });


            await _unitOfWork.GetRepositoryWriteOnly<Bank>().Insert(_mapper.Map<Bank>(bankModel), cancellationToken);

            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Insert,
                Messages = "تم إضافة المصرف",
                NewData = JsonConvert.SerializeObject(bankModel),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }

        public async Task UpdateBank(UpdateBankModel bankModel, CancellationToken cancellationToken = default)
        {
            var oldData = await _unitOfWork.GetRepositoryReadOnly<Bank>().GetByID(bankModel.Id);

            if (oldData == null)
                throw new ApplicationEx("بيانات المصرف غير موجودة");
            var currentUser = _helper.GetCurrentUser();

            var @event = new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Update,
                Messages = "تم تعديل المصرف",
                OldData = JsonConvert.SerializeObject(oldData),
                NewData = JsonConvert.SerializeObject(bankModel),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            };
            await _unitOfWork.GetRepositoryWriteOnly<Bank>().Update(_mapper.Map(bankModel, oldData));

            await _unitOfWork.SaveChangeAsync(cancellationToken);

            await _logginDataPublish.PublishEventData(@event);
        }
    }


}
