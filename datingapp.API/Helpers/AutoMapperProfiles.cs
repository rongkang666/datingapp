using System.Linq;
using AutoMapper;
using datingapp.API.Dtos;
using datingapp.API.Model;

namespace datingapp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
            .ForMember(dest => dest.PhotoUrl, opt => {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p=>p.IsMain).Url);
            })
            .ForMember(dest => dest.Age, opt => {
                opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
            });

            CreateMap<User, UserForDetailDto>().ForMember(dest => dest.PhotoUrl, opt => {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p=>p.IsMain).Url);
            }).ForMember(dest => dest.Age, opt => {
                opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
            });

            CreateMap<Photo,PhotoForDetailDto>();

            CreateMap<UserForUpdateDto, User>();
            
        }
        
    }
}