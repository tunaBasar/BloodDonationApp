using BloodDonationAppUserService.Models.Enums;

namespace BloodDonationAppUserService.Dtos;

public class UserRegisterDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Tc { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Mail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public City City { get; set; }

    public BloodType BloodType { get; set; }

}