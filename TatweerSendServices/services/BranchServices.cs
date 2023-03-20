using AutoMapper;
using FilterAttributeWebAPI.Common;
using Infra;
using Infra.Utili;
using MassTransit;
using Newtonsoft.Json;
using SendEventBus.PublishEvents;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.DTOs.BranchDTOs;
using SharedTatweerSendData.Events;
using SharedTatweerSendData.Models.BranchModels;
using TatweerSendDomain.Domain;
using TatweerSendServices.ExtensionServices;

namespace TatweerSendServices.services
{
    public interface IBranchServices
    {
        Task InsertBranch(InsertBranchModel branch);
        Task InsertBranchList(InsertBranchCollectionModel branch);
        Task UpdateBranch(UpdateBranchModel branch);
        Task ActivationBranch(string branchId, bool isActive);
        Task DeleteBranch(string branchId);
        Task<IReadOnlyList<ActiveBranchDTO>> GetActiveBranchs(string nameOrNumber, string bankRegionId);
        Task<PaginationDto<BranchDTO>> GetBranchs(string nameOrNumber, string bankRegionId, string bankId, int pageNo = 1, int pageSize = 30);


        Task<BranchSettingDTO> GetBranchSetting(string branchSettingId);
        Task UpdateBranchSetting(UpdateBranchSettingModel branchSetting);


        Task<IReadOnlyList<BranchWorkTimeDTO>> GetBranchWorkTimes(string branchId);
        Task UpdateBranchWorkTime(BranchWorkTimeModel branchWorkTime);
        Task ActivationBranchWorkTime(string branchWorkId, bool isActive);
        Task<BranchWorkTimeDTO> GetWorkTimeByBranchIdAndDay(string branchId, DayOfWeek day);

        Task UpdateAllWorkTime(string timeStart, string timeEnd, List<DayOfWeek> days, bool isActive);

    }

    public class BranchServices : IBranchServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly LogginDataPublish _logginDataPublish;
        private readonly HelperUtili _helper;
        public BranchServices(IUnitOfWork unitOfWork, IMapper mapper, LogginDataPublish logginDataPublish, HelperUtili helper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logginDataPublish = logginDataPublish;
            _helper = helper;
        }

        public async Task DeleteBranch(string branchId)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<Branch>().GetByID(branchId);
            if (result == null)
                throw new ApplicationEx("بيانات الفرع غير موجودة");

            await _unitOfWork.GetRepositoryWriteOnly<Branch>().Remove(result);
            await _unitOfWork.SaveChangeAsync();

            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Delete,
                Messages = "تم إلغاء الفرع",
                OldData = JsonConvert.SerializeObject(result),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }

        public async Task<IReadOnlyList<ActiveBranchDTO>> GetActiveBranchs(string nameOrNumber, string bankRegionId)
        => (await _unitOfWork.GetRepositoryReadOnly<Branch>().FindBy(
                predicate: bankRegionId.SearchBranchIsActiveExpression(nameOrNumber),
                selector: select => new ActiveBranchDTO
                {
                    Name = select.Name,
                    BranchNo = select.BranchNo,
                    Id = select.Id
                }
            )).OrderBy(order => order.Name).Take(15).ToList();


        public async Task<PaginationDto<BranchDTO>> GetBranchs(string nameOrNumber, string bankRegionId, string bankId, int pageNo = 1, int pageSize = 30)
        {
            var filterResult = (await _unitOfWork.GetRepositoryReadOnly<Branch>().FindBy(
                    predicate: nameOrNumber.SearchBranchExpression(bankRegionId, bankId),
                    selector: select => new BranchDTO
                    {
                        Id = select.Id,
                        Name = select.Name,
                        BankId = select.BranchRegion.BankId,
                        BankName = select.BranchRegion.Bank.Name,
                        AccountCount = select.Accounts.Count(),
                        BranchNo = select.BranchNo,
                        BranchRegionId = select.BranchRegionId,
                        BranchRegionName = select.BranchRegion.Region.Name,
                        IsActive = select.IsActive,
                        LastCountChekBook = select.LastCountChekBook,
                        LastSerial = select.LastSerial,
                        LastSerialCertified = select.LastSerialCertified,
                        OrderRequestCount = select.OrderRequests.Count(),
                        BranchSettingId = select.BranchSetting.Id,
                        CreateAt = select.CreateAt,
                    },
                    pageNo: pageNo,
                    pageSize: pageSize

                )).OrderByDescending(order => order.CreateAt).ToList();

            var totalRecordCount = await _unitOfWork.GetRepositoryReadOnly<Branch>().GetCount(nameOrNumber.SearchBranchExpression(bankRegionId, bankId));

            return new PaginationDto<BranchDTO>()
            {
                Data = filterResult,
                PageCount = totalRecordCount > 0
            ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
            : 0
            };
        }

        public async Task InsertBranch(InsertBranchModel branch)
        {
            branch.BranchWorkTimes = BranchExtensionServices.GenereteBranchWorkTime();

            await _unitOfWork.GetRepositoryWriteOnly<Branch>().Insert(_mapper.Map<Branch>(branch));
            await _unitOfWork.SaveChangeAsync();
            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Insert,
                Messages = "تم إضافة الفرع",
                NewData = JsonConvert.SerializeObject(branch),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }

        public async Task ActivationBranch(string branchId, bool isActive)
        {
            var oldData = await _unitOfWork.GetRepositoryReadOnly<Branch>().GetByID(branchId);

            if (oldData == null)
                throw new ApplicationEx("بيانات الفرع غير موجودة");

            oldData.IsActive = !isActive;
            await _unitOfWork.SaveChangeAsync();

            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Activation,
                Messages = "تم تغيير حالة الفرع",
                OldData = $"الفرع :{oldData.Name} الحالة : {isActive}",
                NewData = $"الفرع :{oldData.Name} الحالة : {oldData.IsActive}",
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }

        public async Task UpdateBranch(UpdateBranchModel branch)
        {
            var oldData = await _unitOfWork.GetRepositoryReadOnly<Branch>().GetByID(branch.Id);

            if (oldData == null)
                throw new ApplicationEx("بيانات الفرع غير موجودة");

            var currentUser = _helper.GetCurrentUser();
            var @event = new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Update,
                Messages = "تم  تعديل الفرع",
                OldData = JsonConvert.SerializeObject(oldData),
                NewData = JsonConvert.SerializeObject(branch),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            };
            await _unitOfWork.GetRepositoryWriteOnly<Branch>().Update(_mapper.Map(branch, oldData));
            await _unitOfWork.SaveChangeAsync();

            await _logginDataPublish.PublishEventData(@event);
        }

        public async Task InsertBranchList(InsertBranchCollectionModel branch)
        {

            branch.Branchs.All(c =>
            {
                c.BranchRegionId = branch.BranchRegionId;
                c.BranchSetting = BranchExtensionServices.GenerateBranchSetting();
                c.BranchWorkTimes = BranchExtensionServices.GenereteBranchWorkTime();
                return true;
            });

            await _unitOfWork.GetRepositoryWriteOnly<Branch>().InsertList(_mapper.Map<List<Branch>>(branch.Branchs));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task UpdateBranchSetting(UpdateBranchSettingModel branchSetting)
        {
            var oldData = await _unitOfWork.GetRepositoryReadOnly<BranchSetting>().GetByID(branchSetting.Id);

            if (oldData == null)
                throw new ApplicationEx("إعدادات الفرع غير موجودة");

            var currentUser = _helper.GetCurrentUser();

            var @event = new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Update,
                Messages = "تم  تعديل إعدادات الفرع",
                OldData = JsonConvert.SerializeObject(oldData),
                NewData = JsonConvert.SerializeObject(branchSetting),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            };

            await _unitOfWork.GetRepositoryWriteOnly<BranchSetting>().Update(_mapper.Map(branchSetting, oldData));
            await _unitOfWork.SaveChangeAsync();


            await _logginDataPublish.PublishEventData(@event);
        }

        public async Task<BranchSettingDTO> GetBranchSetting(string branchSettingId)
        {
            var data = await _unitOfWork.GetRepositoryReadOnly<BranchSetting>().GetByID(branchSettingId);

            return _mapper.Map<BranchSettingDTO>(data);
        }

        public async Task<IReadOnlyList<BranchWorkTimeDTO>> GetBranchWorkTimes(string branchId)
        => (await _unitOfWork.GetRepositoryReadOnly<BranchWorkTime>().
                FindBy(
                    predicate: pred => pred.BranchId.Equals(branchId),
                    selector: select => new BranchWorkTimeDTO
                    {
                        DayName = select.DayName,
                        Id = select.Id,
                        IsActive = select.IsActive,
                        TimeEnd = select.TimeEnd,
                        TimeStart = select.TimeStart,
                    }
                )).OrderBy(order => order.DayName).ToList();

        public async Task UpdateBranchWorkTime(BranchWorkTimeModel branchWorkTime)
        {
            var currentUser = _helper.GetCurrentUser();

            var oldData = await _unitOfWork.GetRepositoryReadOnly<BranchWorkTime>().GetByID(branchWorkTime.Id);
            var @event = new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Update,
                Messages = "تم  تعديل توقيت الفرع",
                OldData = JsonConvert.SerializeObject(oldData),
                NewData = JsonConvert.SerializeObject(branchWorkTime),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            };
            await _unitOfWork.GetRepositoryWriteOnly<BranchWorkTime>().Update(_mapper.Map(branchWorkTime, oldData));

            await _unitOfWork.SaveChangeAsync();

            await _logginDataPublish.PublishEventData(@event);
        }

        public async Task ActivationBranchWorkTime(string branchWorkId, bool isActive)
        {
            var oldData = await _unitOfWork.GetRepositoryReadOnly<BranchWorkTime>().GetByID(branchWorkId);

            if (oldData == null)
                throw new ApplicationEx("توقيت العمل الفرع غير موجودة");

            oldData.IsActive = !isActive;

            await _unitOfWork.SaveChangeAsync();

            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Activation,
                Messages = "تم  تغيير حالة توقيت عمل الفرع",
                OldData = $"Id : {branchWorkId} IsActive :{isActive}",
                NewData = $"Id : {branchWorkId} IsActive :{oldData.IsActive}",
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }

        public async Task<BranchWorkTimeDTO> GetWorkTimeByBranchIdAndDay(string branchId, DayOfWeek day)
        => (await _unitOfWork.GetRepositoryReadOnly<BranchWorkTime>().FindBy(
                        predicate: pred => pred.BranchId.Equals(branchId) && pred.DayName.Equals(day),
                        selector: select => new BranchWorkTimeDTO
                        {
                            DayName = select.DayName,
                            TimeEnd = select.TimeEnd,
                            TimeStart = select.TimeStart,
                            IsActive = select.IsActive
                        }
                    )).SingleOrDefault();

        public async Task UpdateAllWorkTime(string timeStart, string timeEnd, List<DayOfWeek> days, bool isActive)
        {


            var result = await _unitOfWork.GetRepositoryReadOnly<BranchWorkTime>().FindBy(
                predicate: pred => true,
                selector: select => select
                );

            foreach (var item in days)
            {
                result.Where(w => w.DayName.Equals(item)).All(pred =>
                {
                    pred.TimeStart = timeStart ?? pred.TimeStart;
                    pred.TimeEnd = timeEnd ?? pred.TimeEnd;
                    pred.IsActive = isActive;
                    return true;
                });
            }



            await _unitOfWork.GetRepositoryWriteOnly<BranchWorkTime>().UpdateAll(result);
            await _unitOfWork.SaveChangeAsync();

            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Update,
                Messages = "تم  تغيير جميع توقيت  العمل الفروع",
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }
    }
}
