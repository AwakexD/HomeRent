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
            try
            {
                return Ok(bookingDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the booking.", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPrice(Guid propertyId)
        {
            try
            {
                decimal? pricePerNight = await this.bookingService.GetPropertyPriceAsync(propertyId);

                if (pricePerNight == null)
                {
                    return NotFound(new { message = "Property not found." });
                }

                return Json(new { pricePerNight });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the price.", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBookedDates(Guid propertyId)
        {
            try
            {
                var bookedDateRanges = await bookingService.GetBookedDateRanges(propertyId);
                return Json(new { disable = bookedDateRanges });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving booked dates.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingDto bookingDto)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(this.User);
                if (user == null)
                {
                    return Unauthorized(new { message = "User not found." });
                }

                var bookingId = await this.bookingService.CreateBookingAsync(user.Id, bookingDto);

                if (bookingId == null)
                {
                    return BadRequest(new { message = "Failed to create the booking." });
                }

                var redirectUrl = Url.Action("BookingOverview", "Payment", new { bookingId });

                return Ok(redirectUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the booking.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelBooking(Guid bookingId)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(this.User);
                if (user == null)
                {
                    return Unauthorized(new { message = "User not found." });
                }

                var isCanceled = await this.bookingService.CancelBookingAsync(bookingId, user.Id);

                if (!isCanceled)
                {
                    return NotFound(new { message = "Booking not found or cannot be canceled." });
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while canceling the booking.", error = ex.Message });
            }
        }
    }
}
