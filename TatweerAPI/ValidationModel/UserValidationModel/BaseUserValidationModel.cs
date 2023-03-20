using CollactionData.Models.Users;
using FluentValidation;

namespace IdentityAPI.ValidationModel.UserValidationModel
{
    public class BaseUserValidationModel<T> : AbstractValidator<T> where T : BaseUserModel
    {
        public BaseUserValidationModel()
        {
            RuleFor(rule => rule.Name).NotEmpty().WithMessage("يجب إدخال اسم المستخدم");
            RuleFor(rule => rule.Email).NotEmpty().WithMessage("يجب إدخال بريد الإلكتروني").
                    EmailAddress().WithMessage("بجب إدخال البريد الإلكتروني بطريقة الصحيحة");
            RuleFor(rule => rule.EmployeeNumber).NotEmpty().WithMessage("يجب إدخال رقم تعريف المستخدم");
            RuleFor(rule => rule.UserType).NotEmpty().WithMessage("يجب إختيار تبعية المستخدم");
        }


    }
}
