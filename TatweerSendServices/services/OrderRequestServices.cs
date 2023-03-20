using AutoMapper;
using FilterAttributeWebAPI.Common;
using Infra;
using Infra.Utili;
using Newtonsoft.Json;
using SendEventBus.PublishEvents;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.Events;
using SharedTatweerSendData.Models.OrderRequestModels;
using TatweerSendDomain.Domain;
using TatweerSendServices.ExtensionServices;

namespace TatweerSendServices.services
{
    public interface IOrderRequestServices
    {
        Task InsertOrderRequest(InsertOrderRequestModel model);
        Task UpdateOrderRequest(UpdateOrderRequestModel model);
        Task<PaginationDto<OrderRequestDTO>> GetAllOrderRequest(string userId, UserTypeState? userType, OrderRequestState? requestState,
            BaseAccountType? orderRequestType, string note,
            string branchId, DateTime? from, DateTime? to, int pageNo, int pageSize, bool otherProccess = false, bool isReject = false,
            string IdentityNo = null);
        Task DeleteOrderRequest(string id);
        Task SendOrderRequest(string id, UserTypeState userType);
        Task<OrderRequest> GetById(string id);

        Task ApprovidRequest(string orderRequestId, UserTypeState userType);
        Task RejectRequest(string orderRequestId, string rejectNote, UserTypeState userType);
        Task<RejectNoteDTO> GetRejectNoteByOrderId(string orderRequestId);

    }

    public class OrderRequestServices : IOrderRequestServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GenerateIdentityPublish _sendGenerateIdentity;
        private readonly TrackingOrderEventPublish _sendTrackingOrderEvent;
        private readonly HelperUtili _helper;
        private readonly LogginDataPublish _logginDataPublish;
        public OrderRequestServices(IUnitOfWork unitOfWork, IMapper mapper,
            GenerateIdentityPublish sendGenerateIdentity,
            TrackingOrderEventPublish sendTrackingOrderEvent,
            HelperUtili helper,
            LogginDataPublish logginDataPublish
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sendGenerateIdentity = sendGenerateIdentity;
            _sendTrackingOrderEvent = sendTrackingOrderEvent;
            _helper = helper;
            _logginDataPublish = logginDataPublish;
        }



        public async Task DeleteOrderRequest(string id)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().GetByID(id);

            if (result == null) throw new ApplicationEx("الطلبية تم إلغاءها");

            await _unitOfWork.GetRepositoryWriteOnly<OrderRequest>().Remove(result);
            await _unitOfWork.SaveChangeAsync();
            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.OrderRequest,
                Messages = $"تم إلغاء الطلبية تحت رقم تعريف {id}",

                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }


        public async Task<PaginationDto<OrderRequestDTO>> GetAllOrderRequest(string userId, UserTypeState? userType, OrderRequestState? requestState,
            BaseAccountType? orderRequestType, string note, string branchId,
            DateTime? from, DateTime? to, int pageNo, int pageSize, bool otherProccess, bool isReject = false, string IdentityNo = null)
        {
            var predFilter = otherProccess && !isReject ?
                userType?.SearchOrderRequestByStateExpression(userId, requestState, orderRequestType, note, branchId, IdentityNo) :
                userType?.SearchOrderRequestByUserTypeExpression(userId, requestState, orderRequestType, note, branchId, IdentityNo);

            predFilter = isReject == true ?
                userType?.SearchOrderRequestRejectExpression(userId, requestState, orderRequestType, note, branchId, IdentityNo) :
                predFilter;

            var result = (await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().FindBy(
                  predicate: predFilter,
                  selector: select => new OrderRequestDTO
                  {
                      Id = select.Id,
                      BranchId = select.BranchId,
                      BranchName = select.Branch.Name,
                      RequestDate = select.CreateAt.ToShortDateString(),
                      IdentityNumber = select.IdentityNumber,
                      OrderRequestState = select.OrderRequestState,
                      OrderRequestType = select.OrderRequestType,
                      RegionName = select.Branch.BranchRegion.Region.Name,
                      BranchRegionId = select.Branch.BranchRegion.Id,
                      AccountCount = select.OrderItems.Where(w => w.State.Equals(OrderItemState.Success)).Sum(s => s.CountChekBook),
                      AccountSuspendedCount = select.OrderItems.
                                Where(w => w.State.Equals(OrderItemState.IsSuspended)).Sum(c => c.CountChekBook),
                      CreateAt = select.CreateAt,
                      MinOrderItemBranch = select.OrderRequestType.
                       MinItemInRequestOrderExtensionValidation(
                        select.Branch.BranchSetting.IndividualFrom,
                        select.Branch.BranchSetting.CompanyFrom,
                        select.Branch.BranchSetting.CertifiedFrom)
                  },
                  pageNo: pageNo,
                  pageSize: pageSize
                )).OrderByDescending(order => order.CreateAt).ToList();

            var totalRecordCount = await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().
                                GetCount(predFilter);

            return new PaginationDto<OrderRequestDTO>()
            {
                Data = result,
                PageCount = totalRecordCount > 0
            ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
            : 0
            };

        }

        public async Task<OrderRequest> GetById(string id)
        => await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().GetByID(id);

        public async Task InsertOrderRequest(InsertOrderRequestModel model)
        {

            if (string.IsNullOrEmpty(model.BranchId))
                throw new ApplicationEx("يجب إختيار الفرع");

            var result = _mapper.Map<OrderRequest>(model);

            await _unitOfWork.GetRepositoryWriteOnly<OrderRequest>().Insert(result);
            await _unitOfWork.SaveChangeAsync();

            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.OrderRequest,
                Messages = $"تم إضافة الطلبية تحت رقم تعريف {result.Id}",
                NewData = $"نوع الطلبية : {model.OrderRequestType}",
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });

        }

        public async Task SendOrderRequest(string id, UserTypeState userType)
        {
            var currentUser = _helper.GetCurrentUser();

            var resultOrderRequest = await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().GetByID(id);

            if (resultOrderRequest == null)
                throw new ApplicationEx("الطلبية تم إلغاءها");

            var orderRequestAuthorization = await _unitOfWork.GetRepositoryReadOnly<Branch>().SingalOfDefultAsync(
                predicate: pred => pred.Id.Equals(resultOrderRequest.BranchId),
                selector: select => select.BranchSetting.OrderRequestAuthorization);

            resultOrderRequest.OrderRequestState = userType.GetOrderRequestState(orderRequestAuthorization, resultOrderRequest.OrderRequestState);

            await _unitOfWork.SaveChangeAsync();

            await _sendTrackingOrderEvent.PublishTrackingEvent(
                employeeNo: currentUser?.EmployeeNumber, userId: currentUser?.UserID, publishDate: DateTime.Now, userType: userType,
                orderRequestState: resultOrderRequest.OrderRequestState, orderRequestId: id);


            if (resultOrderRequest.OrderRequestState.ValidateOrderRequestState())
                await _sendGenerateIdentity.PublishGenereteIdentity(
                    orderRequestId: id, employeeNo: currentUser?.EmployeeNumber, UserId: currentUser?.UserID, userType: userType, inputType: InputTypeState.Defualt);


            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.OrderRequest,
                Messages = $"تم إرسال الطلبية تحت رقم تعريف {resultOrderRequest.Id}",
                NewData = $"نوع الطلبية : {resultOrderRequest.OrderRequestType} حالة الطلبية : {resultOrderRequest.OrderRequestState}",
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }

        public async Task UpdateOrderRequest(UpdateOrderRequestModel model)
        {

            var oldData = await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().GetByID(model.Id);



            if (oldData == null)
                throw new ApplicationEx("بيانات الطلبية غير موجودة");

            var currentUser = _helper.GetCurrentUser();

            var @event = new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.OrderRequest,
                Messages = $"تم تعديل الطلبية تحت رقم تعريف {oldData.Id}",
                NewData = JsonConvert.SerializeObject(model),
                OldData = JsonConvert.SerializeObject(oldData),
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            };

            await _unitOfWork.GetRepositoryWriteOnly<OrderRequest>().Update(_mapper.Map(model, oldData));
            await _unitOfWork.SaveChangeAsync();

            await _logginDataPublish.PublishEventData(@event);


        }
        public async Task ApprovidRequest(string orderRequestId, UserTypeState userType)
        {
            var currentUser = _helper.GetCurrentUser();

            var resultOrderRequest = await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().GetByID(orderRequestId);

            if (resultOrderRequest == null)
                throw new ApplicationEx("بيانات الطلبية غير موجودة");

            resultOrderRequest.OrderRequestState = OrderRequestState.GeneretedCode;


            await _unitOfWork.SaveChangeAsync();

            await _sendTrackingOrderEvent.PublishTrackingEvent(
                   employeeNo: currentUser?.EmployeeNumber,
                   userId: currentUser?.UserID,
                   publishDate: DateTime.Now,
                   userType: userType,
                   orderRequestState: resultOrderRequest.OrderRequestState,
                   orderRequestId: orderRequestId
               );

            if (resultOrderRequest.OrderRequestState.ValidateOrderRequestState())
                await _sendGenerateIdentity.PublishGenereteIdentity(
                       orderRequestId: orderRequestId, employeeNo: currentUser?.EmployeeNumber, UserId: currentUser?.UserID, userType: userType, inputType: InputTypeState.Defualt);


            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,

                CreateAt = DateTime.Now,
                EventType = EventTypeState.ApprovedOrderRequest,
                Messages = $"تم قبول الطلبية تحت رقم تعريف {resultOrderRequest.Id}",
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });

        }

        public async Task RejectRequest(string orderRequestId, string rejectNote, UserTypeState userType)
        {
            var currentUser = _helper.GetCurrentUser();
            var resultOrderRequest = await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().GetByID(orderRequestId);

            if (resultOrderRequest == null)
                throw new ApplicationEx("بيانات الطلبية غير موجودة");

            resultOrderRequest.OrderRequestState = OrderRequestState.RejectRequest;


            await _unitOfWork.SaveChangeAsync();

            await _sendTrackingOrderEvent.PublishTrackingEvent(
                   employeeNo: currentUser?.EmployeeNumber,
                   userId: currentUser?.UserID,
                   publishDate: DateTime.Now,
                   userType: userType,
                   orderRequestState: resultOrderRequest.OrderRequestState,
                   orderRequestId: orderRequestId,
                   rejectNote: rejectNote
               );

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,

                CreateAt = DateTime.Now,
                EventType = EventTypeState.RejectOrderRequest,
                Messages = $"تم رفض الطلبية تحت رقم تعريف {resultOrderRequest.Id}",
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });

        }

        public async Task<RejectNoteDTO> GetRejectNoteByOrderId(string orderRequestId)
        {

            var result = await _unitOfWork.GetRepositoryReadOnly<OrderEvent>().FirstOfDefult(
                        predicateOrderBy: pred => pred.OrderCreationDate,
                        predicateWhere: pred =>
                            pred.OrderRequestId == orderRequestId &&
                            !string.IsNullOrEmpty(pred.RejectNote)
                    );

            var data = new RejectNoteDTO
            {
                CreateAtString = result?.OrderCreationDate.ToString("yyyy/MM/dd HH:mm:ss"),
                EmpolyeeNo = result?.EmployeeNo,
                Note = result?.RejectNote
            };

            return data;

        }
    }
}
