using Domain;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace WebApi.Controllers
{
    [Route("mortgages")]
    [ApiController]
    public class MortgageController : ControllerBase
    {
        private readonly IMortgageService _mortgageService;

        public MortgageController(IMortgageService mortgageService)
        {
            _mortgageService = mortgageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mortgage>>> GetMortgageApplications()
        {
            try
            {
                var mortgageApplications = await _mortgageService.GetAllMortgagesAsync();

                return Ok(mortgageApplications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mortgage>> GetMortgageApplicationById(Guid id)
        {
            try
            {
                var mortgageApplication = await _mortgageService.GetMortgageByIdAsync(id);
                    
                return Ok(mortgageApplication);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MortgageDTO>> CreateMortgageApplication([FromBody] MortgageDTO mortgageApplicationDTO)
        {
            try
            {
                var newMortgage = await _mortgageService.CreateMortgageAsync(mortgageApplicationDTO);

                return Ok(newMortgage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
