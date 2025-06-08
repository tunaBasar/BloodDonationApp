using BloodDonationAppUserService.Dtos;
using BloodDonationAppUserService.Response;
using BloodDonationAppUserService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationAppUserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService requestService;
        public RequestController(IRequestService requestService)
        {
            this.requestService = requestService;
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateRequest([FromBody] CreateRequestDto createRequestDto)
        {
            var response = await requestService.CreateRequest(createRequestDto);
            if (response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }
            else
                return StatusCode(response.StatusCode, response);
        }

        [HttpGet("by-bloodtype")]
        public async Task<ActionResult> GetRequestByBloodType([FromQuery] GetRequestDto getRequestDto)
        {
            var response = await requestService.GetRequestByBloodType(getRequestDto);
            if (response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }
            else
                return StatusCode(response.StatusCode, response);
        }

        [HttpGet("by-tc")]
        public async Task<ActionResult> GetRequestsByTcPost([FromQuery] GetRequestDto request)
        {
            try
            {
                var requests = await requestService.GetRequestsByTc(request.Tc);

                if (requests == null || !requests.Data.Any())
                {
                    return NotFound("Bu TC kimlik numarasına ait request bulunamadı.");
                }

                return Ok(requests);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bir hata oluştu: " + ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRequest([FromBody] UpdateRequestDto updateRequestDto)
        {
            try
            {
                var requests = await requestService.UpdateRequest(updateRequestDto);

                if (requests == null)
                {
                    return NotFound("request bulunamadı.");
                }

                return Ok(requests);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bir hata oluştu: " + ex.Message);
            }
        }
    }
}