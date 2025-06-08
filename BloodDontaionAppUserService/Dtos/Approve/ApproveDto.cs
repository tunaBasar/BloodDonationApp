using BloodDonationAppUserService.Models;

namespace BloodDonationAppUserService.Dtos
{
    public class ApproveDto
    {
        public int RequestId { get; set; }

        public string? UserDonorTc { get; set; }

        public string? RequesterTc{ get; set; }
    }
}