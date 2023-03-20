using FluentValidation;
using SharedTatweerSendData.Models.RegionModel;

namespace TatweerSendAPI.ValidationModel.RegionValidationModel
{
    public class UpdateValidationModel : BaseRegionValidationModel<UpdateRegionModel>
    {
        public UpdateValidationModel()
        {
            RuleFor(rule => rule.RegionId).NotEmpty().WithMessage("لم يتم إرسال رقم التعريف المنطقة");
        }
    }
}
