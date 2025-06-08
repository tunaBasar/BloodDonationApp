namespace BloodDonationAppUI.Models
{
    public class CreateRequestModel
    {
        public string Tc { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public BloodType BloodType { get; set; }
        public UrgencyLevel UrgencyLevel { get; set; }
        public City City { get; set; }
    }

    public enum UrgencyLevel
    {
        High,
        Medium,
        Low
    }

}