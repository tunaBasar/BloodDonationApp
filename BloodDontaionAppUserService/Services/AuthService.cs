using BloodDonationAppUserService.Context;
using BloodDonationAppUserService.Dtos;
using BloodDonationAppUserService.Services.Interfaces;
using BloodDonationAppUserService.Helpers;
using Microsoft.EntityFrameworkCore;
using BloodDonationAppUserService.Response;
using BloodDonationAppUserService.Models;

namespace BloodDonationAppUserService.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext context;
    private readonly Converter converter;
    public AuthService(ApplicationDbContext context, Converter converter)
    {
        this.context = context;
        this.converter = converter;
    }

    public  Response<UserResponseDto> LoginUser(UserLoginDto dto)
    {
        var user = FindByTc(dto.Tc);
        var response = converter.UserToResponseDto(user);
        if (user != null && user.Password == dto.Password)
        {
            return Response<UserResponseDto>.SuccessResponse(response, 201);
        }
        else if (user != null && user.Password != dto.Password)
        {
            return Response<UserResponseDto>.FailResponse("Şifre hatalı", 400);
        }
        else
        {
            return Response<UserResponseDto>.FailResponse("Bir şeyler ters gitti", 500);
        }
            
    }

    public async Task<Response<UserResponseDto>> RegisterUser(UserRegisterDto dto)
    {
        try
        {
            var user = converter.ToEntity(dto);
            var exist = await context.Users.AnyAsync(u => u.Tc == dto.Tc && u.Mail == dto.Mail);
            if (exist)
            {
                return Response<UserResponseDto>.FailResponse("Bu Tc yada mail zaten kayıtlı", 400);
            }

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            var response = converter.RegisterDtoToResponseDto(dto);

            return Response<UserResponseDto>.SuccessResponse(response, 201);
        }
        catch (Exception e)
        {
            return Response<UserResponseDto>.FailResponse($"Kayıt sırasında hata oldu: {e.Message}", 500);
        }
    }

    public UserModel FindByTc(string tc)
    {
        var user = context.Users.FirstOrDefault(u => u.Tc == tc);
        if (user == null)
            throw new Exception("Kullanıcı bulunamadı.");
        else
        {
            return user;
        }    
    }

    public async Task<Response<bool>> ForgotPassword(UserForgotPasswordDto dto)
    {
        var user = FindByTc(dto.Tc);
        if (dto.Mail == user.Mail)
        {
            user.Password = dto.Password;
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return Response<bool>.SuccessResponse(true, 200);
        }
        else
        {
            return Response<bool>.FailResponse("aranan mail bulunamadı", 500);
        }
        
    }
}