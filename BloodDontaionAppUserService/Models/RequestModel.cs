using BloodDonationAppUserService.Models.Enums;

namespace BloodDonationAppUserService.Models
{
    public class RequestModel
    {
        public int Id { get; set; }
        public string UserTc { get; set; } = string.Empty;
        public BloodType BloodType { get; set; }
        public UrgencyLevel UrgencyLevel { get; set; }
        public City City { get; set; }
        public bool IsActive { get; set; }=true;
    }
}