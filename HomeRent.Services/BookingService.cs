using AutoMapper;
using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.DTOs.Booking;
using HomeRent.Models.ViewModels.Booking;
using HomeRent.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMapper mapper;

        private readonly IDeletableEntityRepository<Property> propertyReposiotry;
        private readonly IDeletableEntityRepository<Booking> bookingReposotory;

        public BookingService(IMapper mapper,
            IDeletableEntityRepository<Property> propertyReposiotry,
            IDeletableEntityRepository<Booking> bookingRepository)
        {
            this.mapper = mapper;
            this.propertyReposiotry = propertyReposiotry;
            this.bookingReposotory = bookingRepository;
        }

        public async Task<decimal?> GetPropertyPriceAsync(Guid propertyId)
        {
            return await this.propertyReposiotry.AllAsNoTracking()
                .Where(p => p.Id == propertyId)
                .Select(p => (decimal?)p.PricePerNight)
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

        public async Task<Guid?> CreateBookingAsync(Guid userId, CreateBookingDto bookingDto)
        {
            var property = await this.propertyReposiotry.AllAsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == bookingDto.PropertyId);

            if (property == null)
            {
                return null;
            }

            var totalAmount = (bookingDto.CheckOutDate - bookingDto.CheckInDate).Days * property.PricePerNight;

            var booking = new Booking()
            {
                CheckInDate = bookingDto.CheckInDate,
                CheckOutDate = bookingDto.CheckOutDate,
                TotalAmount = totalAmount,
                PropertyId = bookingDto.PropertyId,
                Message = bookingDto.Message,
                TenantId = userId,
                PaymentId = null,
                IsConfirmed = false,

            };

            await bookingReposotory.AddAsync(booking);
            await bookingReposotory.SaveChangesAsync();

            return booking.Id;
        }

        public async Task<BookingOverviewViewModel> GetBookingOverviewAsync(Guid bookingId)
        {
            var booking = await this.bookingReposotory.AllAsNoTracking()
                .Include(b => b.Property)
                .Include(b => b.Property.Owner)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            return this.mapper.Map<BookingOverviewViewModel>(booking);
        }
    }
}
