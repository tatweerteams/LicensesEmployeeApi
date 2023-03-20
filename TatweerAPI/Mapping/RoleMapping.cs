using AutoMapper;
using CollactionData.Models.RoleModel;
using CollactionData.Models.RolePermisstionModel;
using Domain.Domain;

namespace IdentityAPI.Mapping
{
    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<InsertRoleModel, Role>().
             ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString())).
             ForMember(des => des.IsActive, opt => opt.MapFrom(src => true)).
             ForMember(des => des.CreateAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<UpdateRoleModel, Role>().
            ForMember(des => des.ModifyAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<BaseRolePermisstionModel, RolePermisstion>().
               ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));


        }
    }
}
