using BloodDonationAppUserService.Models;

namespace BloodDonationAppUserService.Dtos
{
    public class ApproveDto
    {
        public RequestModel? request { get; set; }

        public string? UserDonorTc { get; set; }
    }
}