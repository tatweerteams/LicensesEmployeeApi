using FluentValidation;
using SharedTatweerSendData.Models.OrderItemModel;

namespace TatweerSendAPI.ValidationModel.OrderItemValidationModel
{
    public class BaseOrderItemValidationModel<T> : AbstractValidator<T> where T : BaseOrderItemModel
    {
        public BaseOrderItemValidationModel()
        {
            RuleFor(rule => rule.AccountId).NotEmpty().WithMessage("لم يتم إرسال رقم التعريف الحساب");
            RuleFor(rule => rule.AccountName).NotEmpty().WithMessage("لم يتم إرسال اسم الحساب");
            RuleFor(rule => rule.AccountNo).NotEmpty().WithMessage("لم يتم إرسال رقم الحساب");
            RuleFor(rule => rule.CountChekBook).NotEmpty().WithMessage("يجب إدخال الكمية الدفاتر");
        }
    }
}
