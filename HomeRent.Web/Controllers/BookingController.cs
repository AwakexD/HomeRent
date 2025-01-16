using HomeRent.Models.DTOs.Booking;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BookingController : Controller
    {
        private readonly IBookingService bookingService;

        public BookingController(IBookingService bookingService) 
        {
            this.bookingService = bookingService;
        }

        [HttpPost]
        public IActionResult Create(CreateBookingDto bookingDto)
        {
            return Ok(bookingDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetPrice(string propertyId)
        {
            if (!Guid.TryParse(propertyId, out var propertyGuid))
            {
                return BadRequest(new { message = "Invalid property ID format." });
            }

            decimal? pricePerNight = await this.bookingService.GetPropertyPriceAsync(propertyGuid);
             
            if (pricePerNight == null)
            {
                return NotFound(new { message = "Property not found." });
            }

            return Json(new { pricePerNight });
        }
    }
}
