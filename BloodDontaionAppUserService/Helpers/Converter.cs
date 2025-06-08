using BloodDonationAppUserService.Dtos;
using BloodDonationAppUserService.Models;

namespace BloodDonationAppUserService.Helpers;

public class Converter
{
    public UserModel ToEntity(UserRegisterDto dto)
    {
        return new UserModel
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Tc = dto.Tc,
            PhoneNumber=dto.PhoneNumber,
            BirthDate = dto.BirthDate,
            Mail = dto.Mail,
            Password = dto.Password,
            City = dto.City,
            BloodType = dto.BloodType
        };
    }

    public UserRegisterDto ToDto(UserModel model)
    {
        return new UserRegisterDto
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Tc = model.Tc,
            PhoneNumber=model.PhoneNumber,
            BirthDate = model.BirthDate,
            Mail = model.Mail,
            Password = model.Password,
            City = model.City,
            BloodType = model.BloodType
        };
    }

    public UserResponseDto RegisterDtoToResponseDto(UserRegisterDto dto)
    {
        return new UserResponseDto
        {
            Tc = dto.Tc,
            City = dto.City,
            PhoneNumber = dto.PhoneNumber,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BloodType = dto.BloodType
        };
    }


    public UserResponseDto UserToResponseDto(UserModel model)
    {
        return new UserResponseDto
        {
            Tc = model.Tc,
            City = model.City,
            PhoneNumber=model.PhoneNumber,
            FirstName = model.FirstName,
            LastName = model.LastName,
            BloodType = model.BloodType
        };
    }

    public RequestModel ToEntity(CreateRequestDto createRequestDto)
    {
        return new RequestModel
        {
            UserTc = createRequestDto.Tc,
            BloodType = createRequestDto.BloodType,
            PhoneNumber=createRequestDto.PhoneNumber,
            City = createRequestDto.City,
            UrgencyLevel = createRequestDto.UrgencyLevel
        };
    }
    public RequestResponseDto ToResponseDto(RequestModel requestModel)
    {
        return new RequestResponseDto
        {
            Id=requestModel.Id,
            Tc = requestModel.UserTc,
            PhoneNumber=requestModel.PhoneNumber,
            BloodType = requestModel.BloodType,
            City = requestModel.City,
            IsActive = requestModel.IsActive,
            UrgencyLevel=requestModel.UrgencyLevel
        };
    }

    public CreateDonationDto ToDto(DonationModel donationModel)
    {
        return new CreateDonationDto
        {
            ApprovementId = donationModel.ApprovementId,
        };
    }

    public DonationModel ToEntity(CreateDonationDto dto)
    {
        return new DonationModel
        {
            ApprovementId = dto.ApprovementId,
        };
    }

    public CreateRequestDto ToDto(RequestModel requestModel)
    {
        return new CreateRequestDto
        {
            Tc = requestModel.UserTc,
            BloodType = requestModel.BloodType,
            City = requestModel.City,
            UrgencyLevel = requestModel.UrgencyLevel
        };
    }

}