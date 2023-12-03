using AutoMapper;
using Work.ApiModels;
using Work.Database;

namespace Work.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserModelDto>()
            .ForMember(u => u.Id, opt => opt.MapFrom(um => um.UserId))
            .ForMember(u => u.Name, opt => opt.MapFrom(um => um.UserName))
            .ForMember(u => u.Birthdate, opt => opt.MapFrom(um => um.Birthday))
            ;
        CreateMap<UserModelDto, User>()
            .ForMember(u => u.UserId, opt => opt.MapFrom(um => um.Id))
            .ForMember(u => u.UserName, opt => opt.MapFrom(um => um.Name))
            .ForMember(u => u.Birthday, opt => opt.MapFrom(um => um.Birthdate))
            ;
    }
}