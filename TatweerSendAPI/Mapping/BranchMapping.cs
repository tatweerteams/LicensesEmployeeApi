using AutoMapper;
using SharedTatweerSendData.DTOs;
using SharedTatweerSendData.Models.BranchModels;
using TatweerSendDomain.Domain;

namespace TatweerSendAPI.Mapping
{
    public class BranchMapping : Profile
    {
        public BranchMapping()
        {
            CreateMap<InsertBranchModel, Branch>().
             ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString())).
               ForMember(des => des.CreateAt, opt => opt.MapFrom(src => DateTime.Now)).
               ForMember(des => des.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<ImportBranchList, Branch>().
             ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString())).
               ForMember(des => des.CreateAt, opt => opt.MapFrom(src => DateTime.Now)).
               ForMember(des => des.IsActive, opt => opt.MapFrom(src => true)).
               ForMember(des => des.LastCountChekBook, opt => opt.MapFrom(src => 0)).
               ForMember(des => des.LastSerial, opt => opt.MapFrom(src => 0)).
               ForMember(des => des.LastSerialCertified, opt => opt.MapFrom(src => 0));


            CreateMap<InsertBranchSettingModel, BranchSetting>().
            ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));

            CreateMap<UpdateBranchSettingModel, BranchSetting>().
             ForMember(des => des.ModifyAt, opt => opt.MapFrom(src => DateTime.Now));


            CreateMap<BranchSetting, BranchSettingDTO>();


            CreateMap<BranchWorkTimeModel, BranchWorkTime>().
             ForMember(des => des.ModifyAt, opt => opt.MapFrom(src => DateTime.Now)).
               ForMember(des => des.IsActive, opt => opt.MapFrom(src => true));


            CreateMap<UpdateBranchModel, Branch>().
             ForMember(des => des.ModifyAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
