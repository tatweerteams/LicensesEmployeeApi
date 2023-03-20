using FluentValidation;
using SharedTatweerSendData.Models.OrderItemModel;

namespace TatweerSendAPI.ValidationModel.OrderItemValidationModel
{
    public class InsertOrderItemValidationModel : AbstractValidator<InsertOrderItemModel>
    {
        public InsertOrderItemValidationModel()
        {
            RuleFor(rule => rule.OrderRequestId).NotEmpty().WithMessage("لم يتم إرسال رقم التعريف الطلبية");
        }
    }

    public class UpdateOrderItemValidationModel : AbstractValidator<UpdateOrderItemModel>
    {
        public UpdateOrderItemValidationModel()
        {
            RuleFor(rule => rule.Id).NotEmpty().WithMessage("لم يتم إرسال رقم التعريف");
        }
    }
}
