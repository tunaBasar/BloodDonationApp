namespace BloodDonationAppUserService.Dtos
{
    public class ApproveResponseDto
    {
        public int Id { get; set; }
        public string UserDonorTc { get; set; } = string.Empty;
        public int? RequestId { get; set; }
        public string? UserRequesterTc { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}