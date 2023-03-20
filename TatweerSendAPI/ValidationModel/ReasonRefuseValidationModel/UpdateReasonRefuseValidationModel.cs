using FluentValidation;
using SharedTatweerSendData.Models.ReasonRefuseModel;

namespace TatweerSendAPI.ValidationModel.ReasonRefuseValidationModel
{
    public class UpdateReasonRefuseValidationModel : BaseReasonRefuseValidationModel<UpdateReasonRefuseModel>
    {
        public UpdateReasonRefuseValidationModel()
        {
            RuleFor(rule => rule.Id).NotEmpty().WithMessage("لم يتم إرسال رقم التعريف سبب الرفض");
        }
    }
}
