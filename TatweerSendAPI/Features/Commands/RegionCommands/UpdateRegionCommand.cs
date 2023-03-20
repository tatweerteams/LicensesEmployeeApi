using Infra;
using MediatR;
using SharedTatweerSendData.Models.RegionModel;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.RegionCommands
{
    public class UpdateRegionCommand : IRequest<ResultOperationDTO<bool>>
    {
        public UpdateRegionModel RegionModel { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }

    public class UpdateRegionHandler : IRequestHandler<UpdateRegionCommand, ResultOperationDTO<bool>>
    {
        private readonly IRegionServices _regionServices;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateRegionHandler(IUnitOfWork unitOfWork, IRegionServices regionServices)
        {
            _unitOfWork = unitOfWork;
            _regionServices = regionServices;
        }
        public async Task<ResultOperationDTO<bool>> Handle(UpdateRegionCommand request, CancellationToken cancellationToken)
        {
            await _regionServices.UpdateRegion(request.RegionModel, request.CancellationToken);


            return ResultOperationDTO<bool>.
                CreateSuccsessOperation(message: new string[] { "تمت العملية التعديل بنجاح" });
        }
    }
}
