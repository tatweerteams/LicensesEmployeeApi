using AutoMapper;
using CollactionData.Models.PermisstionModel;
using Domain.Domain;

namespace IdentityAPI.Mapping
{
    public class PermisstionMapping : Profile
    {
        public PermisstionMapping()
        {
            CreateMap<InsertPermisstionModel, Permisstion>().
               ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString())).
               ForMember(des => des.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<UpdatePermisstionModel, Permisstion>();
        }
    }
}
