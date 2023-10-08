using Domain.DTOs;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace WebApi.Controllers
{
    [Route("customers")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public UserController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllUsers()
        {
            try
            {
                var users = await _customerService.GetAllCustomersAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetUserById(Guid id)
        {
            try
            {
                var user = await _customerService.GetCustomerByIdAsync(id);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateUser([FromBody] CustomerDTO userDTO)
        {
            try
            {
                var newUser = await _customerService.CreateCustomer(userDTO);

                return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
