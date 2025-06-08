using BloodDonationAppUserService.Models.Enums;

namespace BloodDonationAppUserService.Dtos
{
    public class CreateRequestDto
    {
        public string Tc { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public BloodType BloodType { get; set; }
        public UrgencyLevel UrgencyLevel { get; set; }
        public City City { get; set; }
    }
}