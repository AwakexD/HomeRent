using HomeRent.Models.ViewModels;
using HomeRent.Services.Administration.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Areas.Admin.Controllers
{
    public class BookingPaymentsController : AdministrationBaseController
    {
        private readonly IBookingPaymentsService bookingPaymentsService;

        public BookingPaymentsController(IBookingPaymentsService bookingPaymentsService)
        {
            this.bookingPaymentsService = bookingPaymentsService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var bookingAndPayments = await this.bookingPaymentsService.GetAllBookingsAndPaymentsAsync();

                return this.View(bookingAndPayments);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Възникна грешка при взимането на резервациите и плащанията." });
            }
        }
    }
}
