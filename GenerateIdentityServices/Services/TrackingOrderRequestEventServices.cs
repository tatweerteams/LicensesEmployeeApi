using AutoMapper;
using Infra;
using SharedTatweerSendData.Events;
using TatweerSendDomain.Domain;

namespace GenerateIdentityServices.Services
{
    public interface ITrackingOrderRequestEventServices
    {
        Task InsertOrderRequestEvent(TrackingOrderEvent orderEvent);
    }

    public class TrackingOrderRequestEventServices : ITrackingOrderRequestEventServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TrackingOrderRequestEventServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task InsertOrderRequestEvent(TrackingOrderEvent orderEvent)
        {
            var result = _mapper.Map<OrderEvent>(orderEvent);
            await _unitOfWork.GetRepositoryWriteOnly<OrderEvent>().Insert(result);
            await _unitOfWork.SaveChangeAsync();

        }
    }
}
