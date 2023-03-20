using FluentValidation;
using SharedTatweerSendData.Models.Accounts;

namespace TatweerSendAPI.ValidationModel.AccountValidationModel
{
    public class InsertAccountValidationModel : BaseAccountValidationModel<InsertAccountModel>
    {
        public InsertAccountValidationModel()
        {
            RuleFor(rule => rule.PhoneNumber)
                .Must(BeEmptyOrJustNumbers).WithMessage(" رقم الهاتف يجب ان يكون ارقام فقط")
                .Must(BeEmptyOrValidPhonenumber).WithMessage(" رقم الهاتف غير صحيح");

            RuleFor(rule => rule.InputType).IsInEnum().WithMessage("يوجد خطأ في طريقة الادخال");
            RuleFor(rule => rule.AccountType).IsInEnum().WithMessage("يوجد خطأ في نوع الحساب");
            RuleFor(rule => rule.AccountState).IsInEnum().WithMessage("يوجد خطأ في  حالة الحساب");

        }

        private bool BeEmptyOrValidPhonenumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return true;

            if (input.Length != 10) return false;

            if (
                input.StartsWith("092") || input.StartsWith("091") || input.StartsWith("094") ||
                input.StartsWith("095") || input.StartsWith("021") || input.StartsWith("023") ||
                input.StartsWith("025") || input.StartsWith("024") || input.StartsWith("053") ||
                input.StartsWith("051") || input.StartsWith("054") || input.StartsWith("057") ||
                input.StartsWith("064") || input.StartsWith("061") || input.StartsWith("067") ||
                input.StartsWith("069") || input.StartsWith("063") || input.StartsWith("062") || input.StartsWith("071")
                ) { }
            else { return false; }

            return true;
        }

        private bool BeEmptyOrJustNumbers(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return true;

            if (long.TryParse(input, out long value)) return true;

            return false;
        }
    }
}