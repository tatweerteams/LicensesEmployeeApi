using CollactionData.Models.AuthUserModel;
using FluentValidation;

namespace IdentityAPI.ValidationModel.LoginValidationModel
{
    public class LoginUserValidationModel : AbstractValidator<LoginUserModel>
    {
        public LoginUserValidationModel()
        {
            RuleFor(rule => rule.NameOrNumber).NotEmpty().WithMessage("يجب إدخال بريد الإلكتروني أو رقم الوظيفي");
            RuleFor(rule => rule.Password).NotEmpty().WithMessage("يجب إدخال كلمة المرور");
        }
    }
}
