using BloodDonationAppUI.Models;

namespace BloodDonationAppUI.Services.Interfaces
{
    public interface IApiService
    {
        Task<UserCreateModel> RegisterUser(UserCreateModel userCreateModel);
        Task<Response<UserResponseModel>> LoginUser(UserLoginModel userLoginModel);

        Task<UserForgotPassword> ForgotPassword(UserForgotPassword userForgotPassword);

        Task<Response<CreateRequestModel>> CreateRequest(CreateRequestModel createRequestModel);
        Task<Response<List<RequestResponseModel>>> GetRequestByBloodType(int bloodType);

        Task<Response<List<RequestResponseModel>>> GetRequestByTc(GetRequestsByTcModel getRequestsByTcModel);

        Task<Response<T>> SendDonationRequestAsync<T>(DonationRequest request);
    }
}