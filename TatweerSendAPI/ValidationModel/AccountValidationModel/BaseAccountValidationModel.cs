using FluentValidation;
using SharedTatweerSendData.Models.Accounts;

namespace TatweerSendAPI.ValidationModel.AccountValidationModel
{
    public class BaseAccountValidationModel<T> : AbstractValidator<T> where T : baseAccountModel
    {
        public BaseAccountValidationModel()
        {
            RuleFor(rule => rule.AccountNo)
                .NotEmpty().WithMessage("يجب ادخال رقم الحساب ")
                .Must(BeJustNumbers).WithMessage("يجب ادخال رقم الحساب ارقام فقط ")
                .MinimumLength(14).WithMessage("رقم الحساب يجب ان يكون اكبر من 14 رقم");

            RuleFor(rule => rule.AccountName)
                .NotEmpty().WithMessage("يجب ادخال اسم الحساب ")
                .MinimumLength(6).WithMessage("طول الاسم قصير جدا");

            //RuleFor(rule => rule.BranchId).NotEmpty().WithMessage("يجب اختيار الفرع ");
        }

        private bool BeJustNumbers(string input)
        {
            if (long.TryParse(input, out long value)) return true;
            return false;
        }
    }
}