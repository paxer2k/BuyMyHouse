using Domain;

namespace Service.Interfaces
{
    public interface IHouseService
    {
        Task<IEnumerable<House>> GetAllHouses();
        Task<House> GetHouseByIdAsync(Guid id);
        Task<IEnumerable<House>> GetHousesWithinPriceRange(decimal min, decimal max);
    }
}
