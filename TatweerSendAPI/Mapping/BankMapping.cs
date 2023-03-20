using AutoMapper;
using SharedTatweerSendData.Models;
using TatweerSendDomain.Domain;

namespace TatweerSendAPI.Mapping
{
    public class BankMapping : Profile
    {
        public BankMapping()
        {
            CreateMap<InsertBankModel, Bank>().
             ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));

            CreateMap<UpdateBankModel, Bank>().
             ForMember(des => des.ModifyAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
