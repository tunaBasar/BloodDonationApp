using BloodDonationAppUserService.Models.Enums;

namespace BloodDonationAppUserService.Dtos
{
    public class RequestResponseDto
    {
        public int Id{ get; set; }
        public string Tc { get; set; } = string.Empty;
        public BloodType BloodType { get; set; }
        public UrgencyLevel UrgencyLevel { get; set; }
        public City City { get; set; }

        public bool IsActive { get; set; }
    }
}