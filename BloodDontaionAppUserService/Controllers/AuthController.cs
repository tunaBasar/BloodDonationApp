using BloodDonationAppUserService.Dtos;
using BloodDonationAppUserService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BloodDonationAppUserService.Response;

namespace BloodDonationAppUserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterDto dto)
        {
            var response = await authService.RegisterUser(dto);
            if (response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }
            else
                return StatusCode(response.StatusCode, response);
        }

        [HttpPost("login")]
        public  ActionResult LoginUser([FromBody] UserLoginDto dto)
        {
            var response = authService.LoginUser(dto);
            if (response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }
            else
                return StatusCode(response.StatusCode, response);
        }
        [HttpPost("forgotpassword")]
        public async Task<ActionResult> ForgotPassword([FromBody] UserForgotPasswordDto dto)
        {
            var response = await authService.ForgotPassword(dto);
            if (response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }
            else
                return StatusCode(response.StatusCode, response);
        }
    }
}