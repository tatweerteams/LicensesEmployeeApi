using CollactionData.Models.RoleModel;
using CollactionData.Models.RolePermisstionModel;
using FluentValidation;

namespace IdentityAPI.ValidationModel.RoleValidationModel
{
    public class BaseValidationModel<T> : AbstractValidator<T> where T : BaseRoleModel
    {
        public BaseValidationModel()
        {
            RuleFor(rule => rule.Name).NotEmpty().WithMessage("يجب إدخال اسم الدور");
            RuleFor(rule => rule.UserType).NotEmpty().WithMessage("يجب اختيار نوع المستخدم");
            RuleFor(rule => rule.RolePermisstions).Must(checkValue).WithMessage("يجب اختيار صلاحية واحدة علي الأقل");
        }

        private bool checkValue(List<BaseRolePermisstionModel> permisstions)
        {
            try
            {
                return permisstions.Any();
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
