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
        public async Task<IActionResult> GetPrice(Guid propertyId)
        {
            decimal? pricePerNight = await this.bookingService.GetPropertyPriceAsync(propertyId);
             
            if (pricePerNight == null)
            {
                return NotFound(new { message = "Property not found." });
            }

            return Json(new { pricePerNight });
        }

        [HttpGet]
        public async Task<IActionResult> GetBookedDates(Guid propertyId)
        {
            var bookedDateRanges = await bookingService.GetBookedDateRanges(propertyId);

            return Json(new { disable = bookedDateRanges });
        }
    }
}
