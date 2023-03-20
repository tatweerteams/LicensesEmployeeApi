using AutoMapper;
using FilterAttributeWebAPI.Common;
using Infra;
using Infra.Utili;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SendEventBus.PublishEvents;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.Events;
using SharedTatweerSendData.Models.OrderItemModel;
using TatweerSendDomain.Domain;
using TatweerSendServices.ExtensionServices;

namespace TatweerSendServices.services
{
    public interface IOrderRequestItemServices
    {
        Task InsertOrderItem(InsertOrderItemModel orderItem);
        Task UpdateOrderItem(UpdateOrderItemModel orderItem);

        Task<OrderItemResultDTO> GetOrderItems(string orderRequestId, string accounNoOrName,
            string serialFrom, int? quentity, OrderItemState? orderItemState, int pageNo = 1, int pageSize = 30);
        Task DeleteOrderItem(string orderItemId);
        Task<PaginationDto<AccountWithOutOrderItemDTO>> GetAccountWithOutOrderItem(string orderRequestId,
            string accounNoOrName, int pageNo = 1, int pageSize = 30);

        Task ChangeItemState(string orderItemId, UserTypeState userType);

    }

    public class OrderRequestItemServices : IOrderRequestItemServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly LogginDataPublish _logginDataPublish;
        private readonly HelperUtili _helper;
        public OrderRequestItemServices(IUnitOfWork unitOfWork, IMapper mapper, LogginDataPublish logginDataPublish, HelperUtili helper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logginDataPublish = logginDataPublish;
            _helper = helper;
        }


        public async Task DeleteOrderItem(string orderItemId)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<OrderItem>().GetByID(orderItemId);

            if (result == null)
                throw new ApplicationEx("بيانات الحساب غير موجودة في القائمة");

            await _unitOfWork.GetRepositoryWriteOnly<OrderItem>().Remove(result);

            await _unitOfWork.SaveChangeAsync();

            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Delete,
                Messages = $"{result.OrderRequestId}تم  عملية إلغاء الحساب من داخل الطلبية تحت رقم  تعريف الطلبية  ",
                OldData = $"اسم الحساب :{result.AccountName} رقم الحساب : {result.AccountNo}",
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }

        public async Task<PaginationDto<AccountWithOutOrderItemDTO>> GetAccountWithOutOrderItem(string orderRequestId, string accounNoOrName,
            int pageNo = 1, int pageSize = 20)
        {
            var resultOrderRequest = (await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().
               GetByID(orderRequestId));

            if (resultOrderRequest == null)
                throw new ApplicationEx("بيانات الطلب لقد تم إلغاءه");

            var data = (await _unitOfWork.GetRepositoryReadOnly<Account>().FindBy(
                    predicate: orderRequestId.SearchAccountOrderRequestItemExpression(accounNoOrName, resultOrderRequest),
                    selector: select => new AccountWithOutOrderItemDTO
                    {
                        AccountName = select.AccountName,
                        AccountNo = select.AccountNo,
                        AccountId = select.Id,

                        IndividualRequestAccountDay = resultOrderRequest.OrderRequestType.Equals(BaseAccountType.Individual) ?
                                                select.Branch.BranchSetting.IndividualRequestAccountDay : null,

                        IsInsert = select.OrderItems.Any(a => a.OrderRequestId == orderRequestId),
                        CountChekBook = 1,

                        RequestQuantity = resultOrderRequest.OrderRequestType.Equals(BaseAccountType.Individual) ?
                            (select.Branch.BranchSetting.IndividualQuentityOfDay - select.OrderItems.
                            Where(w => w.AccountId.Equals(select.Id) && EF.Functions.DateDiffDay(w.CreateAt, DateTime.Now) == 0).
                            Sum(s => s.CountChekBook)) : null
                    },
                    pageNo: pageNo,
                    pageSize: pageSize

                )).OrderByDescending(order => order.AccountNo).ToList();


            var totalRecordCount = await _unitOfWork.GetRepositoryReadOnly<Account>().
                                    GetCount(orderRequestId.SearchAccountOrderRequestItemExpression(accounNoOrName, resultOrderRequest));


            return new PaginationDto<AccountWithOutOrderItemDTO>()
            {
                Data = data,
                PageCount = totalRecordCount > 0 ?
                            (int)Math.Ceiling(totalRecordCount / (double)pageSize) : 0
            };

        }

        public async Task<OrderItemResultDTO> GetOrderItems(string orderRequestId, string accounNoOrName,
            string serialFrom, int? quentity, OrderItemState? orderItemState, int pageNo = 1, int pageSize = 30)
        {

            var resultOrderRequest = await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().SingalOfDefultAsync(
                    predicate: pred => pred.Id == orderRequestId,

                    selector: select => new BaseOrderRequestDTO
                    {
                        OrderRequestType = select.OrderRequestType,
                        BranchId = select.BranchId,
                        BranchName = select.Branch.Name,
                        Id = select.Id,
                        IdentityNumber = select.IdentityNumber,
                        OrderRequestState = select.OrderRequestState,
                        MinOrderItemBranch = select.OrderRequestType.MinItemInRequestOrderExtensionValidation(select.Branch.BranchSetting.IndividualFrom,
                            select.Branch.BranchSetting.CompanyFrom, select.Branch.BranchSetting.CertifiedFrom),
                        AccountCount = select.OrderItems.Count(),
                    });


            if (resultOrderRequest == null)
                throw new ApplicationEx("بيانات الطلب لقد تم إلغاءه");

            var filterResult = (await _unitOfWork.GetRepositoryReadOnly<OrderItem>().
                FindBy(
                    predicate: orderRequestId.SearchOrderRequestItemExpression(accounNoOrName, quentity, orderItemState),

                    selector: select => new OrderItemDTO
                    {
                        AccountName = select.AccountName,
                        AccountNo = select.AccountNo,
                        AccountId = select.AccountId,
                        CreateAt = select.CreateAt.ToShortDateString(),
                        Id = select.Id,
                        CountChekBook = select.CountChekBook,
                        OrderRequestId = select.OrderRequestId,
                        SerialFrom = select.SerialFrom,
                        State = select.State,
                        CreateDate = select.CreateAt
                    },
                    pageNo: pageNo,
                    pageSize: pageSize

                )).OrderBy(order => order.SerialFrom).ThenByDescending(order => order.CreateDate).ToList();

            var totalRecordCount = await _unitOfWork.GetRepositoryReadOnly<OrderItem>().
                                GetCount(orderRequestId.SearchOrderRequestItemExpression(accounNoOrName, quentity, orderItemState));

            return new OrderItemResultDTO
            {
                OrderRequest = resultOrderRequest,
                OrderItems = filterResult,
                PageCount = totalRecordCount > 0
                ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
                : 0
            };

        }

        public async Task InsertOrderItem(InsertOrderItemModel orderItem)
        {

            await _unitOfWork.GetRepositoryWriteOnly<OrderItem>().Insert(_mapper.Map<OrderItem>(orderItem));
            await _unitOfWork.SaveChangeAsync();

            var currentUser = _helper.GetCurrentUser();

            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Insert,
                Messages = $"{orderItem.OrderRequestId}  تم  عملية إضافة الحساب داخل الطلبية تحت رقم تعريف الطلبية",
                NewData = $"اسم الحساب :{orderItem.AccountName} رقم الحساب : {orderItem.AccountNo}",
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });

        }

        public async Task UpdateOrderItem(UpdateOrderItemModel orderItem)
        {
            var oldData = await _unitOfWork.GetRepositoryReadOnly<OrderItem>().GetByID(orderItem.Id);

            if (oldData == null)
                throw new ApplicationEx("بيانات الحساب غير موجودة في القائمة");

            var currentUser = _helper.GetCurrentUser();

            var @event = new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Update,
                Messages = $"{orderItem.OrderRequestId}  تم  عملية تعديل الكمية دفاتر للحساب  للطلبية تحت رقم تعريف",
                NewData = $"اسم الحساب :{orderItem.AccountName} رقم الحساب : {orderItem.AccountNo} , الكمية : {orderItem.CountChekBook}",
                OldData = $"اسم الحساب :{orderItem.AccountName} رقم الحساب : {orderItem.AccountNo} , الكمية : {oldData.CountChekBook}",
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            };
            await _unitOfWork.GetRepositoryWriteOnly<OrderItem>().Update(_mapper.Map(orderItem, oldData));
            await _unitOfWork.SaveChangeAsync();


            await _logginDataPublish.PublishEventData(@event);
        }

        public async Task ChangeItemState(string orderItemId, UserTypeState userType)
        {
            var result = await _unitOfWork.GetRepositoryReadOnly<OrderItem>().GetByID(orderItemId);

            if (result == null)
                throw new ApplicationEx("بيانات الحساب غير موجودة في القائمة");

            result.State = result.State.ChangeOrderItemState();
            await _unitOfWork.SaveChangeAsync();

            var currentUser = _helper.GetCurrentUser();
            var state = result.State == OrderItemState.IsSuspended ? "مقبول" : "تم تجميد";
            await _logginDataPublish.PublishEventData(new LogginDataEvent
            {
                BranchNumber = currentUser.BranchNumber,
                CreateAt = DateTime.Now,
                EventType = EventTypeState.Activation,
                Messages = $"{result.OrderRequestId}  تم  عملية تغيير حالة دفتر للطلبية تحت رقم تعريف الطلبية",
                NewData = $"اسم الحساب :{result.AccountName} رقم الحساب : {result.AccountNo} , حالة الحساب : ${state}",
                UserId = currentUser.UserID,
                UserName = currentUser.UserName,
                UserType = currentUser.UserType.Value,
            });
        }


    }

}
