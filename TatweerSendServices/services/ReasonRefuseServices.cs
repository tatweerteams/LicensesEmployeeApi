using AutoMapper;
using FilterAttributeWebAPI.Common;
using Infra;
using Infra.Utili;
using Newtonsoft.Json;
using SendEventBus.PublishEvents;
using SharedTatweerSendData.DTOs.ReasonRefuseDTOs;
using SharedTatweerSendData.Events;
using SharedTatweerSendData.Models.ReasonRefuseModel;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.services
{
    public interface IReasonRefuseServices
    {
        Task InsertReasonRefuse(InsertReasonRefuseModel model);
        Task UpdateReasonRefuse(UpdateReasonRefuseModel model);
        Task DeleteReasonRefuse(int id);
        Task<PaginationDto<ReasonRefuseDTO>> GetReasonRefuses(string name, int pageNo, int pageSize);
        Task<IReadOnlyList<ActiveReasonRefuseDTO>> GetActiveReasonRefuses(string name);
        Task ActivationReasonRefuse(int id, bool isActive);


    }

    public class ReasonRefuseServices : IReasonRefuseServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly LogginDataPublish _logginDataPublish;
        private readonly HelperUtili _helper;
        public ReasonRefuseServices(IUnitOfWork unitOfWork, IMapper mapper, LogginDataPublish logginDataPublish, HelperUtili helper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logginDataPublish = logginDataPublish;
            _helper = helper;
        }
        public async Task ActivationReasonRefuse(int id, bool isActive)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<ReasonRefuse>().GetByID(id);
            if (result == null)
                throw new ApplicationEx("بيانات سبب الرفض غير موجودة");

            result.IsActive = !isActive;

            await _unitOfWork.GetRepositoryWriteOnly<ReasonRefuse>().Update(result);

            await _unitOfWork.SaveChangeAsync();
            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Activation,
                Messages = $"تم تغيير حالة سبب الرفض تحت رقم تعريف {id}",
                OldData = $"سبب الرفض :{result.Name} الحالة : {isActive}",
                NewData = $"سبب الرفض :{result.Name} الحالة : {result.IsActive}",
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });

        }

        public async Task DeleteReasonRefuse(int id)
        {
            var currentUser = _helper.GetCurrentUser();


            var result = await _unitOfWork.GetRepositoryReadOnly<ReasonRefuse>().GetByID(id);
            if (result == null)
                throw new ApplicationEx("بيانات سبب الرفض غير موجودة");

            await _unitOfWork.GetRepositoryWriteOnly<ReasonRefuse>().Remove(result);
            await _unitOfWork.SaveChangeAsync();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Delete,
                Messages = $"تم  إلغاء سبب الرفض تحت رقم تعريف  : {id}",
                OldData = JsonConvert.SerializeObject(result),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });

        }

        public async Task<IReadOnlyList<ActiveReasonRefuseDTO>> GetActiveReasonRefuses(string name)
        => await _unitOfWork.GetRepositoryReadOnly<ReasonRefuse>().FindBy(
                    predicate: pred => pred.IsActive == true && (string.IsNullOrWhiteSpace(name) || pred.Name.Contains(name)),
                    selector: select => new ActiveReasonRefuseDTO
                    {
                        Id = select.Id,
                        Name = select.Name,
                    },
                    pageNo: 1,
                    pageSize: 15
                );



        public async Task<PaginationDto<ReasonRefuseDTO>> GetReasonRefuses(string name, int pageNo, int pageSize)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<ReasonRefuse>().FindBy(
                    predicate: pred => string.IsNullOrWhiteSpace(name) || pred.Name.Contains(name),
                    selector: select => new ReasonRefuseDTO
                    {
                        Id = select.Id,
                        Name = select.Name,
                        IsActive = select.IsActive,
                        EmployeeNo = select.EmployeeNo,
                        CreateAt = select.CreateAt.ToString("yyyy/MM/DD")
                    },
                    pageNo: pageNo,
                    pageSize: pageSize
                );


            var totalRecordCount = await _unitOfWork.GetRepositoryReadOnly<ReasonRefuse>().
                GetCount(pred => string.IsNullOrWhiteSpace(name) || pred.Name.Contains(name));

            return new PaginationDto<ReasonRefuseDTO>()
            {
                Data = result,
                PageCount = totalRecordCount > 0
                ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
                : 0
            };
        }

        public async Task InsertReasonRefuse(InsertReasonRefuseModel model)
        {
            var currentUser = _helper.GetCurrentUser();

            var result = _mapper.Map<ReasonRefuse>(model);
            await _unitOfWork.GetRepositoryWriteOnly<ReasonRefuse>().Insert(result);
            await _unitOfWork.SaveChangeAsync();
            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Insert,
                Messages = $"تم  إضاقة سبب الرفض تحت رقم تعريف  : {result.Id}",
                NewData = JsonConvert.SerializeObject(model),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }

        public async Task UpdateReasonRefuse(UpdateReasonRefuseModel model)
        {
            var currentUser = _helper.GetCurrentUser();


            var oldData = await _unitOfWork.GetRepositoryReadOnly<ReasonRefuse>().GetByID(model.Id);

            if (oldData == null)
                throw new ApplicationEx("بيانات سبب الرفض غير موجودة");
            var @event = new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Update,
                Messages = $"تم  تعديل سبب الرفض تحت رقم تعريف  : {oldData.Id}",
                NewData = JsonConvert.SerializeObject(model),
                OldData = JsonConvert.SerializeObject(oldData),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            };
            await _unitOfWork.GetRepositoryWriteOnly<ReasonRefuse>().Update(_mapper.Map(model, oldData));
            await _unitOfWork.SaveChangeAsync();

            await _logginDataPublish.PublishEventData(@event);
        }
    }
}
