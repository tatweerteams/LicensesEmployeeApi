using AutoMapper;
using SharedTatweerSendData.Models.Accounts;
using TatweerSendDomain.Domain;

namespace TatweerSendAPI.Mapping
{
    public class AccountMapping : Profile
    {
        public AccountMapping()
        {
            CreateMap<InsertAccountModel, Account>().
             ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));

            CreateMap<UpdateAccountModel, Account>().
            ForMember(des => des.ModifyAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}