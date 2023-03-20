using AutoMapper;
using LogginDomain;
using SharedTatweerSendData.Events;

namespace LogginWebAPI.Mapping
{
    public class LogginMapping : Profile
    {
        public LogginMapping()
        {
            CreateMap<LogginDataEvent, LogginData>().
             ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));

        }
    }
}
