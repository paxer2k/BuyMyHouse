using AutoMapper;
using Domain;
using Domain.DTOs;

namespace Service.Profiles
{
    public class MortgageProfile : Profile
    {
        public MortgageProfile()
        {
            CreateMap<Mortgage, MortgageDTO>();
            CreateMap<MortgageDTO, Mortgage>();
        }
    }
}
