using CollactionData.Models.PermisstionModel;
using FluentValidation;

namespace IdentityAPI.ValidationModel.PermisstionValidationModel
{
    public class InsertPermisstionVlidationModel : BasePermisstionVlidation<InsertPermisstionModel>
    {
    }

    public class UpdatePermisstionVlidationModel : BasePermisstionVlidation<UpdatePermisstionModel>
    {
        public UpdatePermisstionVlidationModel()
        {
            RuleFor(rule => rule.Id).NotEmpty().WithMessage("لم يتم إرسال رقم التعريف الصلاحية");
        }
    }


}
