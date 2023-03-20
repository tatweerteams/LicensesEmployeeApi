using CollactionData.Models.Users;
using FluentValidation;

namespace IdentityAPI.ValidationModel.UserValidationModel
{
    public class InsertUserValidationModel : BaseUserValidationModel<InsertUserModel>
    {
        public InsertUserValidationModel()
        {
            RuleFor(rule => rule.PasswordHash).NotEmpty().WithMessage("يجب إصدار كلمة المرور");
        }
    }

    public class UpdateUserValidationModel : BaseUserValidationModel<UpdateUserModel>
    {
        public UpdateUserValidationModel()
        {
            RuleFor(rule => rule.Id).NotEmpty().WithMessage("لم يتم إرسال رقم التعريف المستخدم");
        }
    }
}
