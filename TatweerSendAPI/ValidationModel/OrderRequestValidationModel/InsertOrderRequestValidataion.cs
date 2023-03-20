using FluentValidation;
using SharedTatweerSendData.Models.OrderRequestModels;

namespace TatweerSendAPI.ValidationModel.OrderRequestValidationModel
{
    public class InsertOrderRequestValidataion : AbstractValidator<InsertOrderRequestModel>
    {
        public InsertOrderRequestValidataion()
        {
            RuleFor(rule => rule.OrderRequestType).NotEmpty().WithMessage("يجب إختيار نوع الطلب");
        }
    }
}
