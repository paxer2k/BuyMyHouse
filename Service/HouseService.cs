using AutoMapper;
using DAL.Repository.Interfaces;
using Domain;
using Service.Exceptions;
using Service.Interfaces;

namespace Service
{
    public class HouseService : IHouseService
    {
        private readonly IRepository<House> _repository;
        private readonly IMapper _mapper;

        public HouseService(IRepository<House> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<House>> GetAllHouses()
        {
            var houses = await _repository.GetAllAsync();

            if (houses == null)
                throw new BadRequestException("The houses could not be retrieved!");

            return houses;
        }

        public async Task<House> GetHouseByIdAsync(Guid id)
        {
            var house = await _repository.GetByConditionAsync(h => h.Id == id);

            if (house == null)
                throw new BadRequestException($"The house with id {id} does not exist!");

            return house;
        }
    }
}
