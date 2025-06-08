using BloodDonationAppUI.Models;
using BloodDonationAppUserService;

namespace BloodDonationAppUI.Services.Interfaces
{
    public interface IApiService
    {
        Task<UserCreateModel> RegisterUser(UserCreateModel userCreateModel);
        Task<Response<UserResponseModel>> LoginUser(UserLoginModel userLoginModel);

        Task<UserForgotPassword> ForgotPassword(UserForgotPassword userForgotPassword);

        Task<Response<CreateRequestModel>> CreateRequest(CreateRequestModel createRequestModel);
        Task<Response<List<RequestResponseModel>>> GetRequestByBloodType(GetRequests getRequests);

        Task<Response<List<RequestResponseModel>>> GetRequestByTc(GetRequests getRequests);

        Task<Response<T>> SendDonationRequestAsync<T>(DonationRequest request);

        Task<Response<List<ApproveDto>>> GetMyApprovementsByTc(string RequesterTc);
        Task<Response<bool>> CreateDonationAsync(CreateDonationDto createDonationDto);
        Task<Response<bool>> RejectApprove(int ApproveId);
        Task<Response<bool>> UpdateRequest(UpdateRequestDto updateRequestDto);

        Task<Response<bool>> DeleteRequest(int requestId);
    }
}