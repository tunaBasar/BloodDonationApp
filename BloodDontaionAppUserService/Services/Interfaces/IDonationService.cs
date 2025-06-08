using BloodDonationAppUserService.Dtos;
using BloodDonationAppUserService.Response;

namespace BloodDonationAppUserService.Services.Interfaces
{
    public interface IDonationService
    {
        Task<Response<ApproveDto>> SendDonationRequest(MakeDonation makeDonation);
        Task<Response<bool>> CreateDonation(CreateDonationDto donationDto);
    }
}