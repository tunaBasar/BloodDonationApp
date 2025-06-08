using BloodDonationAppUserService.Models.Enums;

namespace BloodDonationAppUserService.Dtos
{
    public class UserResponseDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Tc { get; set; } = string.Empty;
        public BloodType BloodType { get; set; }
        public City City { get; set; }
    }
}