using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IBookingService bookingService;

        public PaymentController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        public async Task<IActionResult> BookingOverview(Guid bookingId)
        {
            return this.View();
        }
    }
}
