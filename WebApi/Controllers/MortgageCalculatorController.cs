using Microsoft.AspNetCore.Mvc;
using Service.Command.Interfaces;
using Service.Interfaces;

namespace WebApi.Controllers
{
    [Route("mortgage-calculators")]
    [ApiController]
    public class MortgageCalculatorController : ControllerBase
    {
        private readonly ICalculateMortgageCommandService _calculateMortgageCommandService;

        public MortgageCalculatorController(ICalculateMortgageCommandService calculateMortgageCommandService)
        {
            _calculateMortgageCommandService = calculateMortgageCommandService;
        }

        [HttpPost]
        public async Task<ActionResult> CalculateMortgages()
        {
            try
            {
                await _calculateMortgageCommandService.CalculateMortgagesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
