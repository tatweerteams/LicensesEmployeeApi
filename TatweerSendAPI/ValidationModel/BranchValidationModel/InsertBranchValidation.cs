using FluentValidation;
using SharedTatweerSendData.Models.BranchModels;

namespace TatweerSendAPI.ValidationModel.BranchValidationModel
{
    public class InsertBranchValidation : BaseBranchValidation<InsertBranchModel>
    {
        public InsertBranchValidation()
        {
            RuleFor(rule => rule.BranchRegionId).NotEmpty().WithMessage("يجب إختيار المنطقة");
        }


    }
}
