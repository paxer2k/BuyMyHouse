using Microsoft.AspNetCore.Mvc;
using Service.Commands.Interfaces;

namespace WebApi.Controllers
{
    [Route("emails")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailCommandService _emailCommandService;

        public EmailController(IEmailCommandService emailCommandService)
        {
            _emailCommandService = emailCommandService;
        }

        [HttpPost]
        public async Task<ActionResult> SendEmails()
        {
            try
            {
                await _emailCommandService.SendEmailsAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
