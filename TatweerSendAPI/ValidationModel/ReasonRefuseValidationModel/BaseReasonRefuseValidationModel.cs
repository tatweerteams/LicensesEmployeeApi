using FluentValidation;
using SharedTatweerSendData.Models.ReasonRefuseModel;

namespace TatweerSendAPI.ValidationModel.ReasonRefuseValidationModel
{
    public class BaseReasonRefuseValidationModel<T> : AbstractValidator<T> where T : BaseReasonRefuseModel
    {
        public BaseReasonRefuseValidationModel()
        {
            RuleFor(rule => rule.Name).NotEmpty().WithMessage("يجب إدخال سبب الرفض");
        }
    }
}
