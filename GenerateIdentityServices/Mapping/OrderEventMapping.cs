using AutoMapper;
using SharedTatweerSendData.Events;
using TatweerSendDomain.Domain;

namespace GenerateIdentityServices.Mapping
{
    public class OrderEventMapping : Profile
    {
        public OrderEventMapping()
        {
            CreateMap<TrackingOrderEvent, OrderEvent>().
                ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        }
    }
}
