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

        public IActionResult Create()
        {
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetPrice(string propertyId)
        {
            decimal pricePerNight = await this.bookingService.GetPropertyPriceAsync(new Guid(propertyId));

            return Json(new { pricePerNight });
        }
    }
}
