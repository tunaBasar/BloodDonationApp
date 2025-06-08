namespace BloodDonationAppUserService.Models
{
    public class DonationModel
    {
        public int Id { get; set; }
        public int ApprovementId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
