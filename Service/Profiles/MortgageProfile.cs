using AutoMapper;
using Domain;
using Domain.DTOs;

namespace Service.Profiles
{
    public class MortgageProfile : Profile
    {
        public MortgageProfile()
        {
            CreateMap<Mortgage, MortgageDTO>()
                .ForMember(dest => dest.Customers, opt => opt.MapFrom(src => src.Customers))
                .ReverseMap(); // If you want to map from MortgageDTO to Mortgage as well

            CreateMap<Mortgage, MortgageResponseDTO>()
                .ForMember(dest => dest.Customers, opt => opt.MapFrom(src => src.Customers))
                .ReverseMap(); // If you want to map from MortgageDTO to Mortgage as well

            CreateMap<Customer, CustomerDTO>();
        }
    }
}
