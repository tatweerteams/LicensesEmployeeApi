using FilterAttributeWebAPI.Common;
using Infra;
using System.Linq.Expressions;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.ExtensionServices
{
    public static class OrderItemExtensionServices
    {
        public static Expression<Func<OrderItem, bool>> SearchOrderRequestItemExpression(this string orderRequestId,
            string accounNoOrName, int? quentity, OrderItemState? orderItemState)
            => pred =>
                    pred.OrderRequestId.Equals(orderRequestId) &&
                    (string.IsNullOrWhiteSpace(accounNoOrName) || (pred.AccountNo.Trim().Contains(accounNoOrName.Trim()) ||
                    pred.AccountName.Trim().Contains(accounNoOrName.Trim()))) &&
                    (quentity == null || pred.CountChekBook.Equals(quentity)) &&
                    (orderItemState == null || pred.State.Equals(orderItemState));


        public static Expression<Func<Account, bool>> SearchAccountOrderRequestItemExpression(this string orderRequestId,
            string accounNoOrName, OrderRequest orderRequest)
            => pred =>
                    !pred.OrderItems.Any(a => a.OrderRequestId == orderRequestId) &&
                    pred.AccountState.Equals(AccountState.IsActive) &&
                    pred.AccountType.Equals(orderRequest.OrderRequestType) &&
                    (orderRequest.PrintOutCenter ? pred.PrintExternally.Equals(true) : true) &&
                    pred.BranchId.Equals(orderRequest.BranchId) &&
                    (string.IsNullOrWhiteSpace(accounNoOrName) || (pred.AccountNo.Trim().Contains(accounNoOrName.Trim()) ||
                    pred.AccountName.Trim().Contains(accounNoOrName.Trim())));

        public static OrderItemState ChangeOrderItemState(this OrderItemState orderItemState)
            => orderItemState switch
            {
                OrderItemState.Success => OrderItemState.IsSuspended,
                OrderItemState.IsSuspended => OrderItemState.Success,
                _ => throw new ApplicationEx("يجب مراجعة مسؤول علي النظام")
            };




    }
}
