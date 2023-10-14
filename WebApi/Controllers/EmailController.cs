using Domain;
using Domain.DTOs;
using Domain.Overview;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace WebApi.Controllers
{
    [Route("emails")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<ActionResult> SendEmails()
        {
            try
            {
                await _emailService.SendEmails();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
