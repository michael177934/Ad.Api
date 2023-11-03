using Ad.API.Resources.Auth;
using Ad.Core.Models;
using AutoMapper;

namespace Tarvcent.API.Mappings
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterResource, ApplicationUser>()
                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s => s.Email))
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.Email))
                .ForMember(d => d.CreatedDate, opt => opt.MapFrom(s => DateTime.UtcNow));



            //CreateMap<Transaction, ApplicationUser>()
            //    .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s => s.Email))
            //    .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.Email))
            //    .ForMember(d => d.CreatedDate, opt => opt.MapFrom(s => DateTime.UtcNow));

        }
    }
}