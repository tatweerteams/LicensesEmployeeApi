using AutoMapper;
using SharedTatweerSendData.Models.RegionModel;
using TatweerSendDomain.Domain;

namespace TatweerSendAPI.Mapping
{
    public class RegionMapping : Profile
    {

        public RegionMapping()
        {
            CreateMap<InsertRegionModel, Region>().
             ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));

            CreateMap<UpdateRegionModel, Region>().
            ForMember(des => des.ModifyAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
