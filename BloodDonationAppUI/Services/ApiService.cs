
using System.Text;
using System.Text.Json;
using BloodDonationAppUI.Models;
using BloodDonationAppUI.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Windows.System;

namespace BloodDonationAppUI.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            baseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5211/api";

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

        public async Task<Response<List<RequestResponseModel>>> GetRequestByBloodType(int bloodType)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/Request/by-bloodtype?bloodType={bloodType}");
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

        public async Task<Response<List<RequestResponseModel>>> GetRequestByTc(GetRequestsByTcModel getRequestsByTcModel)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/Request/by-tc?tc={getRequestsByTcModel.Tc}");
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
    }


}