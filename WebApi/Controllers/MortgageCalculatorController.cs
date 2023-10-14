using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace WebApi.Controllers
{
    [Route("mortgage-calculators")]
    [ApiController]
    public class MortgageCalculatorController : ControllerBase
    {
        private readonly IMortgageCalculatorService _mortgageCalculatorService;

        public MortgageCalculatorController(IMortgageCalculatorService mortgageCalculatorService)
        {
            _mortgageCalculatorService = mortgageCalculatorService;
        }

        [HttpPost]
        public async Task<ActionResult> CalculateMortgages()
        {
            try
            {
                await _mortgageCalculatorService.CalculateMortgagesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
