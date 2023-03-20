using FluentValidation;
using SharedTatweerSendData.Models.RegionModel;

namespace TatweerSendAPI.ValidationModel.RegionValidationModel
{
    public class BaseRegionValidationModel<T> : AbstractValidator<T> where T : BaseRegion
    {
        public BaseRegionValidationModel()
        {
            RuleFor(rule => rule.RegionNo).NotEmpty().WithMessage("يجب إدخال رقم المنطقة");
            RuleFor(rule => rule.Name).NotEmpty().WithMessage("يجب إدخال اسم المنطقة");
        }

    }
}
