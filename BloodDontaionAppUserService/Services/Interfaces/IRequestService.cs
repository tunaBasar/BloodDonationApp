using BloodDonationAppUserService.Dtos;
using BloodDonationAppUserService.Response;

namespace BloodDonationAppUserService.Services.Interfaces
{
    public interface IRequestService
    {
        Task<Response<CreateRequestDto>> CreateRequest(CreateRequestDto createRequestDto);
        Task<Response<List<RequestResponseDto>>> GetRequestByBloodType(GetRequestDto getRequestDto);
        Task<Response<List<RequestResponseDto>>> GetRequestsByTc(string Tc);

        Task<Response<bool>> UpdateRequest(UpdateRequestDto updateRequestDto);

        Task<Response<bool>> DeleteRequest(int requestId);
    }
}