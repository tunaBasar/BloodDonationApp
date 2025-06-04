
using BloodDonationAppUserService.Dtos;
using BloodDonationAppUserService.Response;
using Microsoft.AspNetCore.Identity.Data;
namespace BloodDonationAppUserService.Services.Interfaces
{

    public interface IAuthService
    {
        public Task<Response<UserResponseDto>> RegisterUser(UserRegisterDto dto);
        public Response<UserResponseDto> LoginUser(UserLoginDto dto);

        public Task<Response<bool>> ForgotPassword(UserForgotPasswordDto dto);
    }
}