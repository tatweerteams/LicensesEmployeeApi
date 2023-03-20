using FluentValidation;
using SharedTatweerSendData.Models.BranchModels;

namespace TatweerSendAPI.ValidationModel.BranchValidationModel
{
    public class BaseBranchValidation<T> : AbstractValidator<T> where T : BaseBranchModel
    {
        public BaseBranchValidation()
        {
            RuleFor(rule => rule.BranchNo).NotEmpty().WithMessage("يجب إدخال رقم الفرع");
            RuleFor(rule => rule.Name).NotEmpty().WithMessage("يجب إدخال اسم الفرع");
        }
    }
}

