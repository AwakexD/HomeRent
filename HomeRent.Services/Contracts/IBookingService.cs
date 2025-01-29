using HomeRent.Data.Models.Entities;
using HomeRent.Models.DTOs.Booking;
using HomeRent.Models.ViewModels.Booking;

namespace HomeRent.Services.Contracts
{
    public interface IBookingService
    {
        Task<decimal?> GetPropertyPriceAsync(Guid propertyId);

        Task<IEnumerable<BookedDateRangeDto>> GetBookedDateRanges(Guid propertyId);

        Task<Guid?> CreateBookingAsync(Guid userId, CreateBookingDto bookingDto);

        Task<BookingOverviewViewModel> GetBookingOverviewAsync(Guid bookingId, Guid userId);

        Task<decimal> GetBookingTotalAmount(Guid bookingId);

        Task SavePaymentAndConfirmBooking(Guid bookingId, Payment payment);

        Task<bool> CancelBookingAsync(Guid bookingId, Guid userId);

        Task<bool> IsConfirmed(Guid bookingId);
    }
}
