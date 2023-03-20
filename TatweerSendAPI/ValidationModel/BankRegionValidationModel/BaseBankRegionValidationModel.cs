using FluentValidation;
using SharedTatweerSendData.Models;

namespace TatweerSendAPI.ValidationModel.BankRegionValidationModel
{
    public class BaseBankRegionValidationModel<T> : AbstractValidator<T> where T : BankRegionModel
    {
        public BaseBankRegionValidationModel()
        {
            RuleFor(rule => rule.RegionId).NotEmpty().WithMessage("يجب إختيار المنطقة");
        }
    }
}
