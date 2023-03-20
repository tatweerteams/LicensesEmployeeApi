using Infra;
using MediatR;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.RegionCommands
{
    public class DeleteRegionCommand : IRequest<ResultOperationDTO<bool>>
    {
        public string RegionId { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }

    public class DeleteRegionHandler : IRequestHandler<DeleteRegionCommand, ResultOperationDTO<bool>>
    {
        private readonly IRegionServices _regionServices;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteRegionHandler(IRegionServices regionServices, IUnitOfWork unitOfWork)
        {
            _regionServices = regionServices;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultOperationDTO<bool>> Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
        {
            await _regionServices.DeleteRegion(request.RegionId, request.CancellationToken);
            await _unitOfWork.SaveChangeAsync(request.CancellationToken);


            return ResultOperationDTO<bool>.CreateSuccsessOperation(message: new string[] { "تم العملية الحذف بنجاح" });
        }
    }
}
