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
        private readonly ICustomerService _customerService;

        public MortgageController(IMortgageService mortgageService, ICustomerService customerService)
        {
            _mortgageService = mortgageService;
            _customerService = customerService;
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

        // GET: /customers/5
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
        public async Task<ActionResult<Mortgage>> CreateMortgageApplication([FromBody] MortgageDTO mortgageApplicationDTO)
        {
            try
            {
                var newMortgageApplication = await _mortgageService.CreateMortgageAsync(mortgageApplicationDTO);

                return CreatedAtAction(nameof(GetMortgageApplicationById), new { id = newMortgageApplication.Id }, newMortgageApplication);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{customerId}")]
        public async Task<ActionResult<Mortgage>> GetMortgageByCustomerIdAsync(Guid customerId)
        {
            try
            {
                var mortgage = await _mortgageService.GetMortgageByCustomerIdAsync(customerId);

                return Ok(mortgage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
