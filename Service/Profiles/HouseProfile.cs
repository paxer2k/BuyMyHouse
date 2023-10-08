using Domain.DTOs;
using Domain;
using AutoMapper;

namespace Service.Profiles
{
    public class HouseProfile : Profile
    {
        public HouseProfile() 
        {
            CreateMap<House, HouseDTO>();
            CreateMap<HouseDTO, House>();
        }
    }
}
