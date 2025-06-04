using BloodDonationAppUserService.Dtos;
using BloodDonationAppUserService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonationAppUserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonationController : ControllerBase
    {
        private readonly IDonationService donationService;

        public DonationController(IDonationService donationService)
        {
            this.donationService = donationService;
        }

        [HttpPost("send-request")]
        public async Task<IActionResult> SendDonationRequest([FromBody] MakeDonation makeDonation)
        {
            var result = await donationService.SendDonationRequest(makeDonation);
            
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}