namespace BloodDonationAppUI.Models
{
    public class RequestModel
    {
        public int Id { get; set; }
        public string UserTc { get; set; }
        public int BloodType { get; set; }
        public int UrgencyLevel { get; set; }
        public int City { get; set; }
        public bool IsActive { get; set; } = true;
    }
}