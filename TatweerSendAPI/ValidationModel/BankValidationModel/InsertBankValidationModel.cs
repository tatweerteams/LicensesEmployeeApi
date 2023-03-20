using FluentValidation;
using SharedTatweerSendData.Models;

namespace TatweerSendAPI.ValidationModel.BankValidationModel
{
    public class InsertBankValidationModel : BaseBankValidationModel<InsertBankModel>
    {
        public InsertBankValidationModel()
        {
            RuleFor(rule => rule.BankRegions).Must(checklength).WithMessage("يجب إختيار المناطق للمصرف");
        }

        private bool checklength(List<BankRegionModel> bankRegions)
        {
            try
            {
                return bankRegions.Any();
            }
            catch (Exception)
            {

            }

            return false;
        }
    }
}
