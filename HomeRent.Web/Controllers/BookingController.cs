using HomeRent.Data.Models.User;
using HomeRent.Models.DTOs.Booking;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HomeRent.Common;

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

        [HttpGet]
        public async Task<IActionResult> GetPrice(Guid propertyId)
        {
            try
            {
                decimal? pricePerNight = await this.bookingService.GetPropertyPriceAsync(propertyId);

                if (pricePerNight == null)
                {
                    return NotFound(new { message = ErrorConstants.PropertyNotFoundError });
                }

                return Json(new { pricePerNight });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
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
                return StatusCode(500, new { message = ex.Message });
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
                    return Unauthorized(new { message = ErrorConstants.NotFoundUserError });
                }

                var bookingId = await this.bookingService.CreateBookingAsync(user.Id, bookingDto);

                if (bookingId == null)
                {
                    return NotFound( new { message = ErrorConstants.CreateBookingError });
                }

                var redirectUrl = Url.Action("BookingOverview", "Payment", new { bookingId });

                return Ok(redirectUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
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
                    return Unauthorized(new { message = ErrorConstants.NotFoundUserError });
                }

                var isCanceled = await this.bookingService.CancelBookingAsync(bookingId, user.Id);

                if (!isCanceled)
                {
                    return NotFound(new { message = ErrorConstants.CancelBookingError });
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
