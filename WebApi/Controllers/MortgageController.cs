using Domain;
using Domain.DTOs;
using Domain.Overview;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace WebApi.Controllers
{
    [Route("mortgages")]
    [ApiController]
    public class MortgageController : ControllerBase
    {
        private readonly IMortgageQuery _mortgageQuery;
        private readonly IMortgageCommand _mortgageCommad;

        public MortgageController(IMortgageQuery mortgageQuery, IMortgageCommand mortgageCommad)
        {
            _mortgageQuery = mortgageQuery;
            _mortgageCommad = mortgageCommad;
        }

        [HttpGet]
        public async Task<ActionResult<GenericOverview<Mortgage>>> GetMortgageApplications(int startIndex = 0, int endIndex = 9)
        {
            try
            {
                var mortgageApplications = await _mortgageQuery.GetAllMortgagesAsync(startIndex, endIndex);

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
                var mortgageApplication = await _mortgageQuery.GetMortgageByIdAsync(id);
                    
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
                var newMortgage = await _mortgageCommad.CreateMortgageAsync(mortgageApplicationDTO);

                return Ok(newMortgage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
