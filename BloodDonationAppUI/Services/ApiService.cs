
using System.Text;
using System.Text.Json;
using BloodDonationAppUI.Models;
using BloodDonationAppUI.Services.Interfaces;
using BloodDonationAppUserService;
using Microsoft.Extensions.Configuration;
using Windows.Storage.Pickers;

namespace BloodDonationAppUI.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl;
        private readonly string approvementUrl;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            baseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5211/api";
            approvementUrl = configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5179";

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Response<CreateRequestModel>> CreateRequest(CreateRequestModel createRequestModel)
        {
            try
            {
                var json = JsonSerializer.Serialize(createRequestModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{baseUrl}/Request/create", content);
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();

                var user = JsonSerializer.Deserialize<Response<CreateRequestModel>>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return user!;

            }
            catch (Exception ex)
            {
                return new Response<CreateRequestModel>
                {
                    Success = false,
                    Message = $"Talep oluşturulamadı: {ex.Message}"
                };
            }
        }
        public async Task<Response<bool>> CreateDonationAsync(CreateDonationDto createDonationDto)
        {
            var endpoint = $"{baseUrl}/Donation/approvement";

            try
            {
                var json = JsonSerializer.Serialize(createDonationDto);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                var httpResponse = await httpClient.PostAsync(endpoint, content);

                var responseBody = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new Response<bool>
                    {
                        Success = false,
                        Message = $"Sunucu hatası: {(int)httpResponse.StatusCode} - {responseBody}",
                        Data = false
                    };
                }

                return new Response<bool>
                {
                    Success = true,
                    Message = "Bağış başarıyla oluşturuldu.",
                    Data = true
                };
            }
            catch (HttpRequestException httpEx)
            {
                return new Response<bool>
                {
                    Success = false,
                    Message = $"İstek gönderilirken hata oluştu: {httpEx.Message}",
                    Data = false
                };
            }
            catch (Exception ex)
            {
                return new Response<bool>
                {
                    Success = false,
                    Message = $"Beklenmeyen bir hata oluştu: {ex.Message}",
                    Data = false
                };
            }
        }
        public async Task<Response<List<ApproveDto>>> GetMyApprovementsByTc(string RequesterTc)
        {
            try
            {
                var response = await httpClient.GetAsync($"{approvementUrl}/approve/incoming/{RequesterTc}");
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();

                var approvement = JsonSerializer.Deserialize<Response<List<ApproveDto>>>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return approvement!;
            }
            catch (Exception ex)
            {
                return new Response<List<ApproveDto>>
                {
                    Success = false,
                    Message = $"Approvement getirilemedi: {ex.Message}"
                };
            }
        }
        public async Task<UserForgotPassword> ForgotPassword(UserForgotPassword userForgotPassword)
        {
            try
            {
                var json = JsonSerializer.Serialize(userForgotPassword);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{baseUrl}/Auth/forgotpassword", content);

                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<UserForgotPassword>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return user;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Giriş hatası:{ex.Message}");
            }
        }
        public async Task<Response<UserResponseModel>> LoginUser(UserLoginModel userLoginModel)
        {
            try
            {
                var json = JsonSerializer.Serialize(userLoginModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{baseUrl}/Auth/login", content);

                var responseJson = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<Response<UserResponseModel>>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result!;
            }
            catch (Exception ex)
            {
                return new Response<UserResponseModel>
                {
                    Success = false,
                    Message = $"Login failed: {ex.Message}"
                };
            }
        }
        public async Task<UserCreateModel> RegisterUser(UserCreateModel userCreateModel)
        {
            try
            {
                var json = JsonSerializer.Serialize(userCreateModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{baseUrl}/Auth/register", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();

                var user = JsonSerializer.Deserialize<UserCreateModel>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return user;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Kullanıcı oluşturma hatası:{ex.Message}");
            }

        }
        public async Task<Response<List<RequestResponseModel>>> GetRequestByBloodType(GetRequests getRequests)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/Request/by-bloodtype?Tc={getRequests.Tc}&bloodType={getRequests.BloodType}");
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<Response<List<RequestResponseModel>>>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return result!;
            }
            catch (Exception ex)
            {
                return new Response<List<RequestResponseModel>>
                {
                    Success = false,
                    Message = $"İstekler alınamadı: {ex.Message}",
                    Data = null
                };
            }
        }
        public async Task<Response<List<RequestResponseModel>>> GetRequestByTc(GetRequests getRequests)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/Request/by-tc?tc={getRequests.Tc}");
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<Response<List<RequestResponseModel>>>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return result!;
            }
            catch (HttpRequestException httpEx)
            {
                return new Response<List<RequestResponseModel>>
                {
                    Success = false,
                    Message = $"HTTP hatası: {httpEx.Message}",
                    Data = null
                };
            }
            catch (JsonException jsonEx)
            {
                return new Response<List<RequestResponseModel>>
                {
                    Success = false,
                    Message = $"JSON parse hatası: {jsonEx.Message}",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new Response<List<RequestResponseModel>>
                {
                    Success = false,
                    Message = $"İstekler alınamadı: {ex.Message}",
                    Data = null
                };
            }
        }
        public async Task<Response<T>> SendDonationRequestAsync<T>(DonationRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{baseUrl}/donation/send-request", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<Response<T>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return result ?? new Response<T> { Success = true };
                }
                else
                {
                    return new Response<T>
                    {
                        Success = false,
                        Message = $"API Hatası: {response.StatusCode}, İçerik: {responseContent}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new Response<T>
                {
                    Success = false,
                    Message = $"Bağlantı hatası: {ex.Message}"
                };
            }
        }
        public async Task<Response<bool>> RejectApprove(int ApproveId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{approvementUrl}/approve/{ApproveId}");
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                return new Response<bool>
                {
                    Data = true,
                    Message = "Silme işlemi başarılı",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Response<bool>
                {
                    Data = false,
                    Message = "Silme işlemi başarısız!!!",
                    Success = false
                };
            }
        }

        public async Task<Response<bool>> UpdateRequest(UpdateRequestDto updateRequestDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(updateRequestDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync($"{baseUrl}/Request", content);
                if (response.IsSuccessStatusCode)
                {
                    return new Response<bool> { Success = true };
                }
                else
                {
                    return new Response<bool>
                    {
                        Success = false,
                        Message = $"API Hatası: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new Response<bool>
                {
                    Success = false,
                    Message = $"Bağlantı hatası: {ex.Message}"
                };
            }
        }

        public async Task<Response<bool>> DeleteRequest(int requestId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{baseUrl}/Request/{requestId}");
                response.EnsureSuccessStatusCode();
                var responseJson = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return new Response<bool> { Success = true };
                }
                else
                {
                    return new Response<bool>
                    {
                        Success = false,
                        Message = $"API Hatası: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new Response<bool>
                {
                    Success = false,
                    Message = $"Bağlantı hatası: {ex.Message}"
                };
            }
        }
    }
}