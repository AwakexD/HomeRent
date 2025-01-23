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
            try
            {
                var viewModel = await this.bookingService.GetBookingOverviewAsync(bookingId);

                if (viewModel == null)
                {
                    return NotFound();
                }

                return this.View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
