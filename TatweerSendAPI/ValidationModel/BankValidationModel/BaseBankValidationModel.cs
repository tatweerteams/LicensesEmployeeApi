using FluentValidation;
using SharedTatweerSendData.Models;

namespace TatweerSendAPI.ValidationModel.BankValidationModel
{
    public class BaseBankValidationModel<T> : AbstractValidator<T> where T : BaseBankModel
    {
        public BaseBankValidationModel()
        {
            RuleFor(rule => rule.BankNo).NotEmpty().WithMessage("يجب إدخال رقم المصرف");
            RuleFor(rule => rule.Name).NotEmpty().WithMessage("يجب إدخال اسم المصرف");
        }
    }
}
