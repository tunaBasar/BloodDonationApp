using BloodDonationAppUserService.Models.Enums;

namespace BloodDonationAppUserService.Dtos
{
    public class GetRequestDto
    {
        public string Tc { get; set; } = string.Empty;
        public BloodType BloodType { get; set; }
    }
}