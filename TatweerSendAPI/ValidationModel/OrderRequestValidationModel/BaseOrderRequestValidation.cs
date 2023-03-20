using FluentValidation;
using SharedTatweerSendData.Models.OrderRequestModels;

namespace TatweerSendAPI.ValidationModel.OrderRequestValidationModel
{
    public class BaseOrderRequestValidation<T> : AbstractValidator<T> where T : BaseOrderRequestModel
    {
        public BaseOrderRequestValidation()
        {
            RuleFor(rule => rule.OrderRequestType).NotEmpty().WithMessage("يجب إختيار نوع الطلبية");
        }
    }
}
