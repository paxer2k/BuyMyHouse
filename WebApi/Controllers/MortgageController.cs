using Domain.DTOs;
using Domain.Overview;
using Microsoft.AspNetCore.Mvc;
using Service.Commands.Interfaces;
using Service.Queries.Interfaces;

namespace WebApi.Controllers
{
    [Route("mortgages")]
    [ApiController]
    public class MortgageController : ControllerBase
    {
        private readonly IMortgageQueryService _mortgageQueryService;
        private readonly IMortgageCommandService _mortgageCommandService;

        public MortgageController(IMortgageQueryService mortgageQueryService, IMortgageCommandService mortgageCommadService)
        {
            _mortgageQueryService = mortgageQueryService;
            _mortgageCommandService = mortgageCommadService;
        }

        [HttpGet]
        public async Task<ActionResult<GenericOverview<MortgageResponseDTO>>> GetMortgageApplications(int startIndex = 0, int endIndex = 9)
        {
            try
            {
                var mortgageApplications = await _mortgageQueryService.GetAllMortgagesAsync(startIndex, endIndex);

                return Ok(mortgageApplications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MortgageResponseDTO>> GetMortgageApplicationById(Guid id)
        {
            try
            {
                var mortgageApplication = await _mortgageQueryService.GetMortgageByIdAsync(id);
                    
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
                var newMortgage = await _mortgageCommandService.CreateMortgageAsync(mortgageApplicationDTO);

                return Ok(newMortgage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
