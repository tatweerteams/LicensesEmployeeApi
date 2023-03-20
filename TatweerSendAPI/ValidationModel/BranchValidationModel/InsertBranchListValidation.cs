using FluentValidation;
using SharedTatweerSendData.Models.BranchModels;

namespace TatweerSendAPI.ValidationModel.BranchValidationModel
{
    public class InsertBranchListValidation : AbstractValidator<InsertBranchCollectionModel>
    {
        public InsertBranchListValidation()
        {
            RuleFor(rule => rule.Branchs).NotEmpty().WithMessage("لم يتم إرسال قائمة الفروع");
        }
    }
}
