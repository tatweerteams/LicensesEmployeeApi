using FilterAttributeWebAPI.Common;
using Infra;
using System.Linq.Expressions;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.ExtensionServices
{
    public static class OrderRequestExtensionServices
    {

        public static Expression<Func<OrderRequest, bool>> SearchOrderRequestByUserTypeExpression(this UserTypeState userType,
            string userId, OrderRequestState? requestState,
            BaseAccountType? orderRequestType, string note, string branchId, string IdentityNo)
        {

            switch (userType)
            {
                case UserTypeState.SuperAdmin or UserTypeState.AdminBranch or UserTypeState.AdminCenter:
                    return pred => pred.UserId == userId &&
                         (string.IsNullOrEmpty(branchId) ? true : pred.BranchId.Equals(branchId)) &&
                         (string.IsNullOrWhiteSpace(note) || pred.Note.Contains(note)) &&
                         (string.IsNullOrWhiteSpace(IdentityNo) ? true :
                            (pred.IdentityNumber.Contains(IdentityNo) || pred.IdentityNumberBank.Contains(IdentityNo))) &&
                         (orderRequestType == null ? true : pred.OrderRequestType.Equals(orderRequestType)) &&
                         (requestState == null ? true : pred.OrderRequestState.Equals(requestState));

                case UserTypeState.Employee:
                    return pred => pred.UserId == userId &&
                      pred.BranchId.Equals(branchId) &&
                      (string.IsNullOrWhiteSpace(note) || pred.Note.Contains(note)) &&
                        (string.IsNullOrWhiteSpace(IdentityNo) ? true :
                            (pred.IdentityNumber.Contains(IdentityNo) || pred.IdentityNumberBank.Contains(IdentityNo))) &&
                      (orderRequestType == null ? true : pred.OrderRequestType.Equals(orderRequestType)) &&
                      (requestState == null ? true : pred.OrderRequestState.Equals(requestState));
                default:
                    return order => false;
            }
        }

        public static OrderRequestState GetOrderRequestState(this UserTypeState userType, bool orderRequestAuthorization, OrderRequestState orderRequestState)
            => userType switch
            {
                UserTypeState.Employee => orderRequestAuthorization ?
                OrderRequestState.Pinding : OrderRequestState.GeneretedCode,

                UserTypeState.AdminBranch or UserTypeState.SuperAdmin => OrderRequestState.GeneretedCode,

                UserTypeState.AdminCenter => OrderRequestState.GeneretedCode,
                _ => throw new ApplicationEx($"ليس لديك صلاحية التخويل")
            };

        public static bool ValidateOrderRequestState(this OrderRequestState orderRequestState)
            => orderRequestState switch
            {
                OrderRequestState.GeneretedCode => true,
                _ => false
            };

        public static string GetBranchIdOrderRequest(this UserTypeState userType, string branchSearch, string userBranchId)
            => userType switch
            {
                UserTypeState.Employee => string.IsNullOrWhiteSpace(branchSearch) ? userBranchId : branchSearch,
                UserTypeState.AdminBranch or UserTypeState.AdminCenter or UserTypeState.SuperAdmin => string.IsNullOrWhiteSpace(branchSearch) ?
                userBranchId : branchSearch,
                _ => throw new ApplicationEx($"ليس لديك صلاحية ")
            };

        public static bool OrderRequestTypeIndividualValidation(this BaseAccountType orderRequestType)
            => orderRequestType switch
            {
                BaseAccountType.Individual => true,
                BaseAccountType.Companies or BaseAccountType.Certified => false,
                _ => throw new ApplicationEx($"ليس لديك صلاحية إدخال البيانات")
            };


        public static Expression<Func<OrderRequest, bool>> MaxItemInRequestOrderExtensionValidation(this BaseAccountType orderRequestType,
                 string orderRequestId)

        {
            switch (orderRequestType)
            {
                case BaseAccountType.Individual:
                    return pred => pred.Id.Equals(orderRequestId) &&
                    pred.OrderItems.Count() >= pred.Branch.BranchSetting.IndividualTo;

                case BaseAccountType.Companies:
                    return pred => pred.Id.Equals(orderRequestId) &&
                    pred.OrderItems.Count() >= pred.Branch.BranchSetting.CompanyTo;

                case BaseAccountType.Certified:
                    return pred => pred.Id.Equals(orderRequestId) &&
                    pred.OrderItems.Count() >= pred.Branch.BranchSetting.CertifiedTo;
                default:
                    throw new ApplicationEx($"ليس لديك صلاحية إدخال البيانات");
            }

        }

        public static int MinItemInRequestOrderExtensionValidation(this BaseAccountType orderRequestType,
            int individualFrom = 0, int companyFrom = 0, int certifiedFrom = 0)

        {
            switch (orderRequestType)
            {
                case BaseAccountType.Individual:
                    return individualFrom;

                case BaseAccountType.Companies:
                    return companyFrom;

                case BaseAccountType.Certified:
                    return certifiedFrom;
                default:
                    throw new ApplicationEx($"ليس لديك صلاحية إدخال البيانات");
            }

        }


        public static Expression<Func<OrderRequest, bool>> SearchOrderRequestByStateExpression(this UserTypeState userType,
           string userId, OrderRequestState? requestState,
           BaseAccountType? orderRequestType, string note, string branchId, string IdentityNo)
        {

            switch (userType)
            {
                case UserTypeState.SuperAdmin:

                    return pred => pred.OrderRequestState.Equals(requestState) &&
                         (string.IsNullOrEmpty(branchId) ? true : pred.BranchId.Equals(branchId)) &&
                         (string.IsNullOrWhiteSpace(IdentityNo) ? true :
                         (pred.IdentityNumber.Contains(IdentityNo) || pred.IdentityNumberBank.Contains(IdentityNo))) &&
                         (string.IsNullOrWhiteSpace(note) || pred.Note.Contains(note)) &&
                         (orderRequestType == null ? true : pred.OrderRequestType.Equals(orderRequestType));

                case UserTypeState.AdminCenter:

                    return pred => pred.UserId == userId &&
                         pred.OrderRequestState.Equals(requestState) &&
                          (string.IsNullOrWhiteSpace(IdentityNo) ? true :
                         (pred.IdentityNumber.Contains(IdentityNo) || pred.IdentityNumberBank.Contains(IdentityNo))) &&
                         (string.IsNullOrEmpty(branchId) ? true : pred.BranchId.Equals(branchId)) &&
                         (string.IsNullOrWhiteSpace(note) || pred.Note.Contains(note)) &&
                         (orderRequestType == null ? true : pred.OrderRequestType.Equals(orderRequestType));

                case UserTypeState.Employee or UserTypeState.AdminBranch:

                    return pred => pred.BranchId.Equals(branchId) &&
                     (string.IsNullOrWhiteSpace(IdentityNo) ? true :
                         (pred.IdentityNumber.Contains(IdentityNo) || pred.IdentityNumberBank.Contains(IdentityNo))) &&
                      pred.OrderRequestState.Equals(requestState) &&
                      (string.IsNullOrWhiteSpace(note) || pred.Note.Contains(note)) &&
                      (orderRequestType == null ? true : pred.OrderRequestType.Equals(orderRequestType));

                default:
                    return order => false;
            }
        }

        public static Expression<Func<OrderRequest, bool>> SearchOrderRequestRejectExpression(this UserTypeState userType,
           string userId, OrderRequestState? requestState,
           BaseAccountType? orderRequestType, string note, string branchId, string IdentityNo)
        {

            switch (userType)
            {
                case UserTypeState.SuperAdmin:

                    return pred =>
                         (requestState != null ? pred.OrderRequestState.Equals(requestState) :
                         pred.OrderRequestState.Equals(OrderRequestState.IsFrozen) ||
                         pred.OrderRequestState.Equals(OrderRequestState.IsRejectedByCenter) ||
                         pred.OrderRequestState.Equals(OrderRequestState.RejectRequest)) &&

                          (string.IsNullOrWhiteSpace(IdentityNo) ? true :
                         (pred.IdentityNumber.Contains(IdentityNo) || pred.IdentityNumberBank.Contains(IdentityNo))) &&

                         (string.IsNullOrEmpty(branchId) ? true : pred.BranchId.Equals(branchId)) &&
                         (string.IsNullOrWhiteSpace(note) || pred.Note.Contains(note)) &&
                         (orderRequestType == null ? true : pred.OrderRequestType.Equals(orderRequestType));

                case UserTypeState.AdminCenter:

                    return pred => pred.UserId == userId &&
                         (requestState != null ? pred.OrderRequestState.Equals(requestState) :
                         pred.OrderRequestState.Equals(OrderRequestState.IsFrozen) ||
                         pred.OrderRequestState.Equals(OrderRequestState.IsRejectedByCenter) ||
                         pred.OrderRequestState.Equals(OrderRequestState.RejectRequest)) &&

                         (string.IsNullOrWhiteSpace(IdentityNo) ? true :
                            (pred.IdentityNumber.Contains(IdentityNo) || pred.IdentityNumberBank.Contains(IdentityNo))) &&

                         (string.IsNullOrEmpty(branchId) ? true : pred.BranchId.Equals(branchId)) &&
                         (string.IsNullOrWhiteSpace(note) || pred.Note.Contains(note)) &&
                         (orderRequestType == null ? true : pred.OrderRequestType.Equals(orderRequestType));

                case UserTypeState.Employee or UserTypeState.AdminBranch:

                    return pred => pred.BranchId.Equals(branchId) &&
                       (requestState != null ? pred.OrderRequestState.Equals(requestState) :
                         pred.OrderRequestState.Equals(OrderRequestState.IsFrozen) ||
                         pred.OrderRequestState.Equals(OrderRequestState.IsRejectedByCenter) ||
                         pred.OrderRequestState.Equals(OrderRequestState.RejectRequest)) &&

                        (string.IsNullOrWhiteSpace(IdentityNo) ? true :
                            (pred.IdentityNumber.Contains(IdentityNo) || pred.IdentityNumberBank.Contains(IdentityNo))) &&

                        (string.IsNullOrWhiteSpace(note) || pred.Note.Contains(note)) &&
                            (orderRequestType == null ? true : pred.OrderRequestType.Equals(orderRequestType));

                default:
                    return order => false;
            }
        }
    }
}
