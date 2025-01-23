using HomeRent.Models.DTOs.Booking;
using HomeRent.Models.ViewModels.Booking;

namespace HomeRent.Services.Contracts
{
    public interface IBookingService
    {
        Task<decimal?> GetPropertyPriceAsync(Guid propertyId);

        Task<IEnumerable<BookedDateRangeDto>> GetBookedDateRanges(Guid propertyId);

        Task<Guid?> CreateBookingAsync(Guid userId, CreateBookingDto bookingDto);

        Task<BookingOverviewViewModel> GetBookingOverviewAsync(Guid bookingId);
    }
}
