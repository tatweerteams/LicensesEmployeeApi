using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedTatweerSendData.Models.BranchModels;
using TatweerSendServices.servicesValidation;

namespace TatweerSendAPI.Filters.BranchFilter
{
    public class InsertBranchListFilter : ActionFilterAttribute
    {
        private readonly IBranchValidationServices _branchValidation;
        public InsertBranchListFilter(IBranchValidationServices branchValidation)
        {
            _branchValidation = branchValidation;
        }
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.TryGetValue("model", out var _insertListModel);

            if (_insertListModel is InsertBranchCollectionModel insertListModel)
            {
                var nameBranchs = insertListModel.Branchs.Select(s => s.Name).ToList();
                var numberBranchs = insertListModel.Branchs.Select(s => s.BranchNo).ToList();

                if (numberBranchs.GroupBy(s => s).Select(s => s.Count()).Max() > 1 ||
                    nameBranchs.GroupBy(s => s).Select(s => s.Count()).Max() > 1)
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<List<ImportBranchList>>.
                      CreateErrorOperation(new string[] { "هذه القائمة تحتوي علي فروع متكررة" }));

                    return;
                }

                var result = await _branchValidation.
                    IsExistsData(nameBranchs, numberBranchs, insertListModel.BranchRegionId);

                if (numberBranchs.All(a => result.Contains(a)))
                {
                    context.Result = new OkObjectResult(ResultOperationDTO<List<ImportBranchList>>.
                        CreateErrorOperation(new string[] { "قائمة الفروع موجودة مسبقا" }));

                    return;
                }

                insertListModel.ExistBranchs = insertListModel.Branchs.
                            Where(w => result.Contains(w.BranchNo)).ToList();

                if (insertListModel.ExistBranchs.Any())
                    insertListModel.ExistBranchs.All(a => { a.Note = "رقم الفرع أو إسم الفرع موجود مسبقا"; return true; });

                insertListModel.Branchs = insertListModel.Branchs.Where(w => !result.Contains(w.BranchNo)).ToList();

            }

            await base.OnActionExecutionAsync(context, next);
        }


    }
}
