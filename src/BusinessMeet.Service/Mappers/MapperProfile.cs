using AutoMapper;
using BusinessMeet.Domain.Entities;
using BusinessMeet.Service.DTOs.Companys;
using BusinessMeet.Service.DTOs.Meets;
using BusinessMeet.Service.DTOs.UserMeets;
using BusinessMeet.Service.DTOs.Users;

namespace BusinessMeet.Service.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        //User
        CreateMap<User, UserForUpdateDto>().ReverseMap();
        CreateMap<User, UserForResultDto>().ReverseMap();
        CreateMap<User, UserForCreationDto>().ReverseMap();

        //Meet
        CreateMap<Meet, MeetForResultDto>().ReverseMap();
        CreateMap<Meet, MeetForUpdateDto>().ReverseMap();
        CreateMap<Meet, MeetForCreationDto>().ReverseMap();

        //UserMeet
        CreateMap<UserMeet, UserMeetForResultDto>().ReverseMap();
        CreateMap<UserMeet, UserMeetForUpdateDto>().ReverseMap();
        CreateMap<UserMeet, UserMeetForCreationDto>().ReverseMap();

        //Company
        CreateMap<Company, CompanyForResultDto>().ReverseMap();
        CreateMap<Company, CompanyForUpdateDto>().ReverseMap();
        CreateMap<Company, CompanyForCreationDto>().ReverseMap();

    }
}
