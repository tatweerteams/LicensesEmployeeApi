using SharedTatweerSendData.Models.BranchModels;
using System.Linq.Expressions;
using TatweerSendDomain.Domain;

namespace TatweerSendServices.ExtensionServices
{
    public static class BranchExtensionServices
    {

        public static InsertBranchSettingModel GenerateBranchSetting()
        {
            return new InsertBranchSettingModel
            {
                AccountChekBook = string.Empty,
                CertifiedFrom = 1,
                CertifiedTo = 1000,
                CompanyFrom = 1,
                CompanyTo = 1000,
                IndividualFrom = 1,
                IndividualTo = 1000,
                IndividualQuentityOfDay = 5,
                IndividualRequestAccountDay = true,
                BranchRequestCountOfDay = 10,
                OrderRequestAuthorization = true,

            };
        }
        public static List<BranchWorkTimeModel> GenereteBranchWorkTime()
        {
            List<BranchWorkTimeModel> branchWorkTimes = new();

            var maxDay = (int)DayOfWeek.Saturday;

            for (int day = 0; day <= maxDay; day++)
            {
                branchWorkTimes.Add(new BranchWorkTimeModel
                {
                    DayName = (DayOfWeek)day,
                    IsActive = false,
                    Id = Guid.NewGuid().ToString(),
                    TimeEnd = new TimeSpan(15, 0, 0).ToString(),
                    TimeStart = new TimeSpan(8, 0, 0).ToString(),
                });
            }
            return branchWorkTimes;
        }

        public static Expression<Func<Branch, bool>> SearchBranchExpression(this string nameOrNumber,
           string bankRegionId, string bankId)
           => pred => (string.IsNullOrWhiteSpace(nameOrNumber) || (pred.Name.Contains(nameOrNumber.Trim()) ||
                    pred.BranchNo.Contains(nameOrNumber.Trim()))) &&
                (string.IsNullOrEmpty(bankRegionId) || pred.BranchRegionId.Equals(bankRegionId)) &&
                (string.IsNullOrEmpty(bankId) || pred.BranchRegion.BankId.Equals(bankId));

        public static Expression<Func<Branch, bool>> SearchBranchIsActiveExpression(this string bankRegionId, string nameOrNumber)
         => pred => pred.BranchRegionId.Equals(bankRegionId) &&
                    pred.IsActive == true &&
                    (string.IsNullOrWhiteSpace(nameOrNumber) ||
                    pred.BranchNo.Contains(nameOrNumber) || pred.Name.Contains(nameOrNumber));

    }
}
