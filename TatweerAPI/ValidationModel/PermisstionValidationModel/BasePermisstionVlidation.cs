using CollactionData.Models.PermisstionModel;
using FluentValidation;

namespace IdentityAPI.ValidationModel.PermisstionValidationModel
{
    public class BasePermisstionVlidation<T> : AbstractValidator<T> where T : BasePermisstionModel
    {
        public BasePermisstionVlidation()
        {
            RuleFor(rule => rule.Name).NotEmpty().WithMessage("يجب إدخال اسم الصلاحية");
            RuleFor(rule => rule.Description).NotEmpty().WithMessage("يجب إدخال وصف الصلاحية");
        }


    }
}
