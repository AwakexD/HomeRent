using HomeRent.Models.Administration;

namespace HomeRent.Services.Administration.Contracts
{
    public interface IBookingPaymentsService
    {
        Task<IEnumerable<BookingPaymentViewModel>> GetAllBookingsAndPaymentsAsync();
    }
}
