using System.Linq.Expressions;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.ExtensionServices
{
    public static class BankRegionExtensionServices
    {
        public static Expression<Func<BankRegion, bool>> SearchBankRegionExpression(this string bankId,
         string regionNameOrNumber)
         => pred =>
                    (string.IsNullOrEmpty(bankId) || pred.BankId.Equals(bankId)) &&
                    (string.IsNullOrWhiteSpace(regionNameOrNumber) ||
                    (pred.Region.Name.Contains(regionNameOrNumber) ||
                    pred.Region.RegionNo.Contains(regionNameOrNumber))
                    );

        public static Expression<Func<BankRegion, bool>> SearchBankRegionActiveExpression(this string bankId,
        string regionNameOrNumber)
            => pred => pred.IsActive == true &&
                    pred.BankId.Equals(bankId) &&
                    (
                        string.IsNullOrWhiteSpace(regionNameOrNumber) ||
                        (pred.Region.Name.Contains(regionNameOrNumber) ||
                        pred.Region.RegionNo.Contains(regionNameOrNumber))
                    );

    }
}
