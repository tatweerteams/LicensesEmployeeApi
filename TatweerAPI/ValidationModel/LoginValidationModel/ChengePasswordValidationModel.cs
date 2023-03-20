using CollactionData.Models.AuthUserModel;
using FluentValidation;

namespace IdentityAPI.ValidationModel.LoginValidationModel
{
    public class ChengePasswordValidationModel : AbstractValidator<ChengePasswordModel>
    {
        public ChengePasswordValidationModel()
        {
            RuleFor(rule => rule.UserId).NotEmpty().WithMessage("الرجاء محاولة لاحقا");
            RuleFor(rule => rule.EmployeeNumber).NotEmpty().WithMessage("يجب إدخال رقم الوظيفي");
            RuleFor(rule => rule.Password).NotEmpty().WithMessage("يجب إدخال كلمة المرور");
        }
    }
}
