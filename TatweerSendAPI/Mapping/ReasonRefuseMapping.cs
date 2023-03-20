using AutoMapper;
using SharedTatweerSendData.Models.ReasonRefuseModel;
using TatweerSendDomain.Domain;

namespace TatweerSendAPI.Mapping
{
    public class ReasonRefuseMapping : Profile
    {
        public ReasonRefuseMapping()
        {
            CreateMap<InsertReasonRefuseModel, ReasonRefuse>().
               ForMember(des => des.CreateAt, opt => opt.MapFrom(src => DateTime.Now)).
               ForMember(des => des.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<UpdateReasonRefuseModel, ReasonRefuse>().
               ForMember(des => des.ModifyAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
