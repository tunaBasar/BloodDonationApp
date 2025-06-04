using BloodDonationAppUserService.Dtos;
using BloodDonationAppUserService.Response;

namespace BloodDonationAppUserService.Services.Interfaces
{
    public interface IRequestService
    {
        Task<Response<CreateRequestDto>> CreateRequest(CreateRequestDto createRequestDto);
        Task<Response<List<RequestResponseDto>>> GetRequestByBloodType(int bloodType);
        Task<Response<List<RequestResponseDto>>> GetRequestsByTc(string Tc);
    }
}