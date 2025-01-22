using HomeRent.Data.Models.User;
using HomeRent.Models.DTOs.Booking;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BookingController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IBookingService bookingService;

        public BookingController(UserManager<ApplicationUser> userManager, 
            IBookingService bookingService) 
        {
            this.bookingService = bookingService;
            this.userManager = userManager;
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

        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingDto bookingDto)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var bookingId = await this.bookingService.CreateBookingAsync(user.Id, bookingDto);

            if (bookingId == null)
            {
                return BadRequest(new { message = "Failed to create the booking. " });
            }

            //return RedirectToAction("Overview", "Payment", new { bookingId })
            return Ok(bookingId);
        }
    }
}
