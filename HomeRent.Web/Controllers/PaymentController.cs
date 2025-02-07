using HomeRent.Common;
using HomeRent.Data.Models.Entities;
using HomeRent.Data.Models.User;
using HomeRent.Models.ViewModels;
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
                    return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.BookingAlreadyConfirmedError });
                }

                var user = await this.userManager.GetUserAsync(this.User);
                if (user == null)
                {
                    return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.UserNotFoundError });
                }

                var viewModel = await this.bookingService.GetBookingOverviewAsync(bookingId, user.Id);
                if (viewModel == null)
                {
                    return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.BookingNotFoundError });
                }

                ViewBag.HideFooter = true;

                return this.View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.BookingOverviewRetrieveError });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(Guid bookingId, string paymentMethod)
        {
            try
            {
                if (string.IsNullOrEmpty(paymentMethod))
                {
                    var user = await this.userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.UserNotFoundError });
                    }

                    var viewModel = await this.bookingService.GetBookingOverviewAsync(bookingId, user.Id);
                    return View(nameof(BookingOverview), viewModel);
                }

                if (paymentMethod == "cash")
                {
                    var payment = new Payment
                    {
                        Id = Guid.NewGuid(),
                        BookingId = bookingId,
                        StripeTransactionId = "CASH_PAYMENT",
                        AmountPaid = await this.bookingService.GetBookingTotalAmount(bookingId),
                        PaymentDate = DateTime.UtcNow,
                        Status = "Cash Payment",
                    };

                    await this.bookingService.SavePaymentAndConfirmBooking(bookingId, payment);

                    return RedirectToAction("PaymentSuccess", new { bookingId });
                }
                else if (paymentMethod == "card")
                {
                    var domain = $"{Request.Scheme}://{Request.Host}";
                    var successUrl = $"{domain}/Payment/PaymentSuccess?bookingId={bookingId}&sessionId={{CHECKOUT_SESSION_ID}}";
                    var cancelUrl = $"{domain}/Payment/CancelBooking?bookingId={bookingId}";

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

                return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.InvalidPaymentMethodError });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.PaymentProcessError });
            }
        }

        public async Task<IActionResult> PaymentSuccess(Guid bookingId, string? sessionId = null)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    var isConfirmed = await this.bookingService.IsConfirmed(bookingId);
                    if (isConfirmed)
                    {
                        ViewBag.BookingId = bookingId;
                        ViewBag.HideFooter = true;
                        return View();
                    }
                    return RedirectToAction(nameof(PaymentFailure), new { bookingId });
                }

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

                    ViewBag.BookingId = bookingId;
                    ViewBag.HideFooter = true;
                    return View();
                }

                return RedirectToAction(nameof(PaymentFailure), new { bookingId });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.PaymentConfirmationError });
            }
        }

        [HttpGet]
        public IActionResult PaymentFailure(Guid bookingId)
        {
            try
            {
                ViewBag.BookingId = bookingId;
                ViewBag.HideFooter = true;
                return this.View();
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.PaymentFailurePageError });
            }
        }
    }
}
