using Infra;
using MediatR;
using SharedTatweerSendData.Models.BranchModels;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.BranchCommands
{
    public class InsertBranchListCommand : IRequest<ResultOperationDTO<List<ImportBranchList>>>
    {
        public InsertBranchCollectionModel BranchModel { get; set; }
    }

    public class InsertBranchListHandler : IRequestHandler<InsertBranchListCommand, ResultOperationDTO<List<ImportBranchList>>>
    {
        private readonly IBranchServices _branchServices;
        public InsertBranchListHandler(IBranchServices branchServices)
        {
            _branchServices = branchServices;
        }

        public async Task<ResultOperationDTO<List<ImportBranchList>>> Handle(InsertBranchListCommand request, CancellationToken cancellationToken)
        {
            await _branchServices.InsertBranchList(request.BranchModel);

            var isExistsBranchs = request.BranchModel.ExistBranchs.
                Select(s => new ImportBranchList
                {
                    BranchNo = s.BranchNo,
                    BranchRegionId = s.BranchRegionId,
                    Name = s.Name,
                    Note = s.Note,
                }).ToList();

            var massage = isExistsBranchs.Any() ?
                "تم عملية حفظ بعض الفروع , وقائمة هذه الفروع  موجودة مسبقا" :
                "تم عملية حفظ القائمة الفروع";

            return ResultOperationDTO<List<ImportBranchList>>.
                SendResponseWithData(isExistsBranchs, new string[] { massage });
        }
    }
}
