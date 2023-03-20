using AutoMapper;
using SharedTatweerSendData.Models.OrderItemModel;
using TatweerSendDomain.Domain;

namespace TatweerSendAPI.Mapping
{
    public class OrderItemMapping : Profile
    {
        public OrderItemMapping()
        {
            CreateMap<InsertOrderItemModel, OrderItem>().
               ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString())).
               ForMember(des => des.CreateAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<UpdateOrderItemModel, OrderItem>();

        }
    }
}
