namespace BloodDonationAppUI.Models
{
    public class UpdateRequestDto
    {
        public int RequestId { get; set; }
        public City City { get; set; }
        public UrgencyLevel UrgencyLevel { get; set; }
    }
}