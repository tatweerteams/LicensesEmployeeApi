using AutoMapper;
using CollactionData.Models.Users;
using Domain;

namespace IdentityAPI.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<InsertUserModel, User>().
              ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString())).
              ForMember(des => des.IsActive, opt => opt.MapFrom(src => true)).
              ForMember(des => des.SendSMS, opt => opt.MapFrom(src => false)).
              ForMember(des => des.CreateAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<UpdateUserModel, User>().
              ForMember(des => des.ModifyAt, opt => opt.MapFrom(src => DateTime.Now));

        }
    }
}
