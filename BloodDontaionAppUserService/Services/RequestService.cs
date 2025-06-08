using BloodDonationAppUserService.Context;
using BloodDonationAppUserService.Dtos;
using BloodDonationAppUserService.Helpers;
using BloodDonationAppUserService.Models;
using BloodDonationAppUserService.Models.Enums;
using BloodDonationAppUserService.Response;
using BloodDonationAppUserService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationAppUserService.Services
{
    public class RequestService : IRequestService
    {
        private readonly ApplicationDbContext context;
        private readonly Converter converter;
        public RequestService(ApplicationDbContext context, Converter converter)
        {
            this.context = context;
            this.converter = converter;
        }
        public async Task<Response<CreateRequestDto>> CreateRequest(CreateRequestDto createRequestDto)
        {
            try
            {
                var request = converter.ToEntity(createRequestDto);
                await context.Requests.AddAsync(request);
                await context.SaveChangesAsync();
                var response = converter.ToDto(request);
                return Response<CreateRequestDto>.SuccessResponse(response, 201);
            }
            catch (Exception ex)
            {
                return Response<CreateRequestDto>.FailResponse($"Kayıt sırasında hata oldu: {ex.Message}", 500);
            }
        }

        public async Task<Response<List<RequestResponseDto>>> GetRequestByBloodType(GetRequestDto getRequestDto)
        {
            var requests = await context.Requests
                .Where(r => r.BloodType == getRequestDto.BloodType && r.IsActive == true && r.UserTc != getRequestDto.Tc)
                .ToListAsync();

            if (requests == null || !requests.Any())
            {
                return Response<List<RequestResponseDto>>.FailResponse("Uygun talep bulunamadı.", 404);
            }

            var requestDtos = requests.Select(r => converter.ToResponseDto(r)).ToList();
            return Response<List<RequestResponseDto>>.SuccessResponse(requestDtos, 200);
        }


        public async Task<Response<List<RequestResponseDto>>> GetRequestsByTc(string Tc)
        {
            var requests = await context.Requests.Where(r => r.UserTc == Tc && r.IsActive != false).ToListAsync();
            var requestDtos = requests.Select(r => converter.ToResponseDto(r)).ToList();
            if (requests != null)
            {
                return Response<List<RequestResponseDto>>.SuccessResponse(requestDtos, 201);
            }
            return Response<List<RequestResponseDto>>.FailResponse("Talepler bulunamadı", 400);

        }

        public async Task<Response<bool>> UpdateRequest(UpdateRequestDto updateRequestDto)
        {
            try
            {
                var request = await context.Requests.FindAsync(updateRequestDto.RequestId);
                if (request != null)
                {
                    request.City = updateRequestDto.City;
                    request.UrgencyLevel = updateRequestDto.UrgencyLevel;

                    await context.SaveChangesAsync();

                    return Response<bool>.SuccessResponse(true, 201);
                }

                return Response<bool>.FailResponse("request null olamaz",404);
            }
            catch (Exception ex)
            {
                return Response<bool>.FailResponse( $"Bir hata oluştu: {ex.Message}",500);
            }
        }

    }
}