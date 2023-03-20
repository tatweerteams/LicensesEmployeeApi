using AutoMapper;
using SharedTatweerSendData.Models;
using TatweerSendDomain.Domain;

namespace TatweerSendAPI.Mapping
{
    public class BankRegionMapping : Profile
    {
        public BankRegionMapping()
        {
            CreateMap<BankRegionModel, BankRegion>().
                ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString())).
                ForMember(des => des.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<InsertBankRegionModel, BankRegion>().
                ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString())).
                ForMember(des => des.IsActive, opt => opt.MapFrom(src => true));
        }
    }
}
