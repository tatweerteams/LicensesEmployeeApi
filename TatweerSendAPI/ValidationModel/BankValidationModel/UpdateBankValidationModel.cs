using FluentValidation;
using SharedTatweerSendData.Models.BranchModels;
using TatweerSendAPI.ValidationModel.BranchValidationModel;

namespace TatweerSendAPI.ValidationModel.BankValidationModel
{
    public class UpdateBankValidationModel : BaseBranchValidation<UpdateBranchModel>
    {
        public UpdateBankValidationModel()
        {
            RuleFor(rule => rule.Id).NotEmpty().WithMessage("لم يتم إرسال رقم التعريف");
        }
    }
}
