using BloodDonationAppUserService.Context;
using BloodDonationAppUserService.Dtos;
using BloodDonationAppUserService.Helpers;
using BloodDonationAppUserService.Response;
using BloodDonationAppUserService.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace BloodDonationAppUserService.Services
{
    public class DonationService : IDonationService
    {
        private readonly ApplicationDbContext context;
        private readonly HttpClient httpClient;
        private readonly Converter converter;
        private readonly string approveServiceUrl;

        public DonationService(ApplicationDbContext context, HttpClient httpClient, IConfiguration configuration, Converter converter)
        {
            this.httpClient = httpClient;
            approveServiceUrl = configuration["ApiSettings:ApproveServiceUrl"] ?? "http://localhost:5179";

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            this.context = context;
            this.converter = converter;
        }

        public async Task<Response<bool>> CreateDonation(CreateDonationDto donationDto)
        {
            try
            {
                if (donationDto == null)
                {
                    return Response<bool>.FailResponse("Donation bilgisi boş olamaz.", 400);
                }

                if (donationDto.ApprovementId <= 0)
                {
                    return Response<bool>.FailResponse("Geçerli bir Approvement ID giriniz.", 400);
                }

                var request = await context.Requests.FindAsync(donationDto.RequestId);
                if (request == null)
                {
                    return Response<bool>.FailResponse("İlgili bağış talebi bulunamadı.", 404);
                }
                request.IsActive = false;

                var donation = converter.ToEntity(donationDto);
                donation.CreatedAt = DateTime.UtcNow;

                await context.Donations.AddAsync(donation);

                await context.SaveChangesAsync();

                var jsonContent = JsonSerializer.Serialize(donation);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var httpResponse = await httpClient.PutAsync(
                    $"{approveServiceUrl}/approve/confirm/{donationDto.ApprovementId}", content
                );

                if (httpResponse.IsSuccessStatusCode)
                {
                    return Response<bool>.SuccessResponse(true, 201);
                }

                var errorBody = await httpResponse.Content.ReadAsStringAsync();
                return Response<bool>.FailResponse($"Approvement servisi başarısız: {errorBody}", 500);
            }
            catch (Exception ex)
            {
                return Response<bool>.FailResponse($"Sunucu hatası: {ex.Message}", 500);
            }
        }


        public async Task<Response<ApproveDto>> SendDonationRequest(MakeDonation makeDonation)
        {
            try
            {
                if (makeDonation?.ApproveDto == null)
                {
                    return Response<ApproveDto>.FailResponse("Bağış talebi bilgileri eksik", 400);
                }

                if (string.IsNullOrWhiteSpace(makeDonation.ApproveDto.UserDonorTc))
                {
                    return Response<ApproveDto>.FailResponse("Donor TC kimlik numarası gerekli", 400);
                }

                if (makeDonation.ApproveDto.RequestId <= 0)
                {
                    return Response<ApproveDto>.FailResponse("Geçerli bir talep ID'si gerekli", 400);
                }

                var approveRequest = new
                {
                    UserDonorTc = makeDonation.ApproveDto.UserDonorTc,
                    RequestId = makeDonation.ApproveDto.RequestId,
                    UserRequesterTc = makeDonation.ApproveDto.RequesterTc
                };

                var jsonContent = JsonSerializer.Serialize(approveRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{approveServiceUrl}/approve", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var approveResponse = JsonSerializer.Deserialize<ApproveResponseDto>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var resultDto = new ApproveDto
                    {
                        RequesterTc = makeDonation.ApproveDto.RequesterTc,
                        UserDonorTc = makeDonation.ApproveDto.UserDonorTc
                    };

                    return Response<ApproveDto>.SuccessResponse(resultDto, (int)response.StatusCode);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var errorMessage = "Bağış talebi gönderilemedi";

                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<Response<ApproveDto>>(errorContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (!string.IsNullOrEmpty(errorResponse?.Message))
                        {
                            errorMessage = errorResponse.Message;
                        }
                    }
                    catch
                    {
                        if (!string.IsNullOrEmpty(errorContent))
                        {
                            errorMessage = errorContent;
                        }
                    }

                    return Response<ApproveDto>.FailResponse(errorMessage, (int)response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                return Response<ApproveDto>.FailResponse($"Bağlantı hatası: {ex.Message}", 503);
            }
            catch (TaskCanceledException ex)
            {
                return Response<ApproveDto>.FailResponse($"İstek zaman aşımına uğradı: {ex.Message}", 408);
            }
            catch (Exception ex)
            {
                return Response<ApproveDto>.FailResponse($"Beklenmeyen hata: {ex.Message}", 500);
            }
        }
    }

}