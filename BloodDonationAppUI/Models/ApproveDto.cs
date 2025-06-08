namespace BloodDonationAppUI.Models
{
    public class ApproveDto
    {
        public int Id { get; set; }
        public string RequesterTc { get; set; } = string.Empty;
        public string UserDonorTc { get; set; } = string.Empty;
        public required int RequestId { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string Status => IsApproved switch
        {
            null => "Bekliyor",
            true => "Onaylandı",
            false => "Reddedildi"
        };
    }

}