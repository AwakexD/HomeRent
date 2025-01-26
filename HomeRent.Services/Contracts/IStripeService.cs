using Stripe.Checkout;

namespace HomeRent.Services.Contracts
{
    public interface IStripeService
    {
        Session CreateStripeSession(Guid bookingId, decimal amount, string currency, string successUrl, string cancelUrl);
    }
}
