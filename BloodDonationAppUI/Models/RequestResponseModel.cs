using System.Text.Json.Serialization;

namespace BloodDonationAppUI.Models
{
    public class RequestResponseModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        public string Tc { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public BloodType BloodType { get; set; }
        public UrgencyLevel UrgencyLevel { get; set; }
        public City City { get; set; }
        public bool IsActive { get; set; }
    }
}