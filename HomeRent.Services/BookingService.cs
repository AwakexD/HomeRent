using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.DTOs.Booking;
using HomeRent.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Services
{
    public class BookingService : IBookingService
    {
        private readonly IDeletableEntityRepository<Property> propertyReposiotry;
        private readonly IDeletableEntityRepository<Booking> bookingReposotory;

        public BookingService(IDeletableEntityRepository<Property> propertyReposiotry,
            IDeletableEntityRepository<Booking> bookingRepository)
        {
            this.propertyReposiotry = propertyReposiotry;
            this.bookingReposotory = bookingRepository;
        }

        public async Task<decimal> GetPropertyPriceAsync(Guid propertyId)
        {
            return await this.propertyReposiotry.AllAsNoTracking()
                .Where(p => p.Id == propertyId)
                .Select(p => p.PricePerNight)
                .FirstOrDefaultAsync();
        } 

        public async Task<IEnumerable<BookedDateRangeDto>> GetBookedDateRanges(Guid propertyId)
        {
            var bookedDates = await this.bookingReposotory.AllAsNoTracking()
                .Where(b => b.PropertyId == propertyId && b.IsDeleted == false)
                .Select (b => new BookedDateRangeDto
                {
                    From = b.CheckInDate.ToString("dd-MM-yyyy"),
                    To = b.CheckOutDate.ToString("dd-MM-yyyy"),
                })
                .ToListAsync();
                
            return bookedDates;
        }
    }
}
