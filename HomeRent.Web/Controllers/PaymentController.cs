using HomeRent.Data.Models.Entities;
using HomeRent.Data.Models.User;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IBookingService bookingService;
        private readonly IStripeService stripeService;

        public PaymentController(UserManager<ApplicationUser> userManager,
            IBookingService bookingService,
            IStripeService stripeService)
        {
            this.userManager = userManager;
            this.bookingService = bookingService;
            this.stripeService = stripeService;
        }

        public async Task<IActionResult> BookingOverview(Guid bookingId)
        {
            try
            {
                var isConfirmed = await this.bookingService.IsConfirmed(bookingId);

                if (isConfirmed)
                {
                    return this.BadRequest("Booking is already confirmed.");
                }

                var user = await this.userManager.GetUserAsync(this.User);

                var viewModel = await this.bookingService.GetBookingOverviewAsync(bookingId, user.Id);
                if (viewModel == null)
                {
                    return NotFound("Booking not found.");
                }

                return this.View(viewModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(Guid bookingId, string paymentMethod)
        {
            if (string.IsNullOrEmpty(paymentMethod))
            {
                var user = await this.userManager.GetUserAsync(User);
                var viewModel = await this.bookingService.GetBookingOverviewAsync(bookingId, user.Id);

                return View(nameof(BookingOverview), viewModel);
            }

            if (paymentMethod == "cash")
            {
                // ToDO
                // Service method to mark the booking as confirmed
                // Redirect to the PaymentSuccess action

                return NotFound();
            }
            else if (paymentMethod == "card")
            {
                // Initiate Stripe Payment Session
                var domain = $"{Request.Scheme}://{Request.Host}";
                var successUrl = $"{domain}/Payment/PaymentSuccess?bookingId={bookingId}&sessionId={{CHECKOUT_SESSION_ID}}";
                var cancelUrl = $"{domain}/Payment/PaymentCancel?bookingId={bookingId}";

                var totalAmount = await this.bookingService.GetBookingTotalAmount(bookingId);

                var session = this.stripeService.CreateStripeSession(
                bookingId,
                    totalAmount,
                    "BGN",
                    successUrl,
                    cancelUrl
                );

                return Redirect(session.Url);
            }

            return BadRequest("An error occurr while proccessing the payment");
        }

        public async Task<IActionResult> PaymentSuccess(Guid bookingId, string sessionId)
        {
            var service = new Stripe.Checkout.SessionService();
            var session = service.Get(sessionId);

            if (session.PaymentStatus == "paid")
            {
                var payment = new Payment
                {
                    BookingId = bookingId,
                    StripeTransactionId = session.Id,
                    AmountPaid = (session.AmountTotal / 100m),
                    PaymentDate = DateTime.UtcNow,
                    Status = "Succeeded"
                };

                await this.bookingService.SavePaymentAndConfirmBooking(bookingId, payment);

                this.ViewBag.BookingId = bookingId;
                return this.View();
            }
            else
            {
                return this.View(nameof(PaymentFailure), new { bookingId });
            }
        }

        [HttpGet]
        public IActionResult PaymentFailure(Guid bookingId)
        {
            ViewBag.BookingId = bookingId;

            return this.View();
        }
    }
}
