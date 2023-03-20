using Infra;
using MediatR;
using SharedTatweerSendData.Models.RegionModel;
using TatweerSendServices.services;

namespace TatweerSendAPI.Features.Commands.RegionCommands
{
    public class InsertRegionCommand : IRequest<ResultOperationDTO<bool>>
    {
        public InsertRegionModel RegionModel { get; set; }
        public CancellationToken CancellationToken { get; set; } = default;
    }

    public class InsertRegionHandler : IRequestHandler<InsertRegionCommand, ResultOperationDTO<bool>>
    {
        private readonly IRegionServices _regionServices;
        private readonly IUnitOfWork _unitOfWork;
        public InsertRegionHandler(IRegionServices regionServices, IUnitOfWork unitOfWork)
        {
            _regionServices = regionServices;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultOperationDTO<bool>> Handle(InsertRegionCommand request, CancellationToken cancellationToken)
        {
            await _regionServices.AddRegion(request.RegionModel, request.CancellationToken);

            return ResultOperationDTO<bool>.
                CreateSuccsessOperation(message: new string[] { "تمت العملية إضافة بنجاح" });
        }
    }
}
