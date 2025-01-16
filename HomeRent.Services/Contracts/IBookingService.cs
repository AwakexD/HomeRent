using HomeRent.Models.DTOs.Booking;

namespace HomeRent.Services.Contracts
{
    public interface IBookingService
    {
        Task<decimal> GetPropertyPriceAsync(Guid propertyId);

        Task<IEnumerable<BookedDateRangeDto>> GetBookedDateRanges(Guid propertyId);
    }
}
