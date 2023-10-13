using AutoMapper;
using DAL.Repository.Interfaces;
using Domain;
using Service.Exceptions;
using Service.Interfaces;

namespace Service
{
    public class HouseService : IHouseService
    {
        private readonly IRepository<House> _houseRepository;
        private readonly IRepository<Mortgage> _mortgageRepository;
        private readonly IMapper _mapper;

        public HouseService(IRepository<House> houseRepository, IRepository<Mortgage> mortgageRepository, IMapper mapper)
        {
            _houseRepository = houseRepository;
            _mortgageRepository = mortgageRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<House>> GetAllHouses()
        {
            var houses = await _houseRepository.GetAllAsync();

            if (houses == null)
                throw new BadRequestException("The houses could not be retrieved!");

            return houses;
        }

        public async Task<House> GetHouseByIdAsync(Guid id)
        {
            var house = await _houseRepository.GetByConditionAsync(h => h.Id == id);

            if (house == null)
                throw new BadRequestException($"The house with id {id} does not exist!");

            return house;
        }

        public Task<IEnumerable<House>> GetHousesWithinPriceRange(decimal min, decimal max)
        {
            return _houseRepository.GetAllByConditionAsync(h => h.Price >= min && h.Price <= max);
        }        
    }
}
