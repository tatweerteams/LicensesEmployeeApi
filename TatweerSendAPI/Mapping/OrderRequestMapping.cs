using AutoMapper;
using Infra;
using SharedTatweerSendData.Models.OrderRequestModels;
using TatweerSendDomain.Domain;

namespace TatweerSendAPI.Mapping
{
    public class OrderRequestMapping : Profile
    {
        public OrderRequestMapping()
        {
            CreateMap<InsertOrderRequestModel, OrderRequest>().
                ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString())).
                ForMember(des => des.InputTypeState, opt => opt.MapFrom(src => InputTypeState.Defualt)).
                ForMember(des => des.CreateAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<UpdateOrderRequestModel, OrderRequest>().
                ForMember(des => des.LastModifyDate, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
