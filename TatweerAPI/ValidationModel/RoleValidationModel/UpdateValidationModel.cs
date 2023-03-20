using CollactionData.Models.RoleModel;
using FluentValidation;

namespace IdentityAPI.ValidationModel.RoleValidationModel
{
    public class UpdateValidationModel : BaseValidationModel<UpdateRoleModel>
    {
        public UpdateValidationModel()
        {
            RuleFor(rule => rule.Id).NotEmpty().WithMessage("لم يتم إرسال رقم التعريف بيانات الدور");
        }
    }
}
