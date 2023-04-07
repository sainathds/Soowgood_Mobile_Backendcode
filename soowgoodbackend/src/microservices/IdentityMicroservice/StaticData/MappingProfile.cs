using AutoMapper;
using IdentityMicroservice.Model;
using IdentityMicroservice.ViewModels;

namespace IdentityMicroservice.StaticData
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>()
                //.ForMember(
                //    dest => dest.Designation,
                //    opt => opt.MapFrom(src => src.Designation.ToString())
                //)
                .ForMember(
                    dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Gender.ToString())
                )
                .ForMember(
                    dest => dest.MaritalStatus,
                    opt => opt.MapFrom(src => src.MaritalStatus.ToString())
                )
                .ForMember(
                    dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth.ToString("MMMM dd, yyyy"))
                )
                .ForMember(
                    dest => dest.MemberSince,
                    opt => opt.MapFrom(src => src.MemberSince.ToString("MMMM dd, yyyy"))
                )
                .ReverseMap();

            CreateMap<ApplicationUser, UserProfileEditViewModel>()
               
               .ForMember(
                   dest => dest.Gender,
                   opt => opt.MapFrom(src => src.Gender.ToString())
               )
               .ForMember(
                   dest => dest.MaritalStatus,
                   opt => opt.MapFrom(src => src.MaritalStatus.ToString())
               )
               .ForMember(
                   dest => dest.DateOfBirth,
                   opt => opt.MapFrom(src => src.DateOfBirth.ToString("MM/dd/yyyy"))
               )
               .ForMember(
                   dest => dest.MemberSince,
                   opt => opt.MapFrom(src => src.MemberSince.ToString("MMMM dd, yyyy"))
               )
               .ReverseMap();

        }
    }
}
