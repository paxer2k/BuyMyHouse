using Domain;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace WebApi.Controllers
{
    [Route("houses")]
    [ApiController]
    public class HouseController : ControllerBase
    {
        private readonly IHouseService _houseService;

        public HouseController(IHouseService houseService)
        {
            _houseService = houseService;
            //
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<House>>> GetAllHouses()
        {
            try
            {
                var houses = await _houseService.GetAllHouses();

                return Ok(houses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetHouseById(Guid id)
        {
            try
            {
                var house = await _houseService.GetHouseByIdAsync(id);

                return Ok(house);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
