using Domain;
using Domain.DTOs;
using Domain.Overview;
using Microsoft.AspNetCore.Mvc;
using Service.Commands.Interfaces;
using Service.Queries.Interfaces;

namespace WebApi.Controllers
{
    [Route("process-mortgages")]
    [ApiController]
    public class ProcessMortgageController : ControllerBase
    {
        private readonly ICalculateMortgageCommandService _calculateMortgageCommandService;
        private readonly IProcessMortgageCommandService _processMortgageCommandService;

        public ProcessMortgageController(ICalculateMortgageCommandService calculateMortgageCommandService, IProcessMortgageCommandService processMortgageCommandService)
        {
            _calculateMortgageCommandService = calculateMortgageCommandService;
            _processMortgageCommandService = processMortgageCommandService;
        }

        [HttpPost]
        public async Task<ActionResult> ProcessMortgages()
        {
            try
            {
                await _calculateMortgageCommandService.CalculateMortgagesAsync();
                await _processMortgageCommandService.ProcessMortgagesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }  
    }
}
