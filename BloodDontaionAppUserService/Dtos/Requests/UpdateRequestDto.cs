using BloodDonationAppUserService.Models.Enums;

namespace BloodDonationAppUserService.Dtos
{
    public class UpdateRequestDto
    {
        public int RequestId { get; set; }
        public City City { get; set; }
        public UrgencyLevel UrgencyLevel { get; set; }
    }

}