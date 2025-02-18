using AutoMapper;
using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.DTOs.Booking;
using HomeRent.Models.ViewModels.Booking;
using HomeRent.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;
using System.Security.Policy;

namespace HomeRent.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMapper mapper;

        private readonly IDeletableEntityRepository<Property> propertyReposiotry;
        private readonly IDeletableEntityRepository<Booking> bookingReposotory;
        private readonly IDeletableEntityRepository<Payment> paymentReposiotry;

        public BookingService(IMapper mapper,
            IDeletableEntityRepository<Property> propertyReposiotry,
            IDeletableEntityRepository<Booking> bookingRepository,
            IDeletableEntityRepository<Payment> paymentRepository)
        {
            this.mapper = mapper;
            this.propertyReposiotry = propertyReposiotry;
            this.bookingReposotory = bookingRepository;
            this.paymentReposiotry = paymentRepository;
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

        public async Task<BookingOverviewViewModel> GetBookingOverviewAsync(Guid bookingId, Guid userId)
        {
            var booking = await this.bookingReposotory.AllAsNoTracking()
                .Include(b => b.Property)
                .Include(b => b.Property.Owner)
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.TenantId == userId);

            return this.mapper.Map<BookingOverviewViewModel>(booking);
        }

        public async Task<decimal> GetBookingTotalAmount(Guid bookingId)
        {
            var totalAmount = await this.bookingReposotory.AllAsNoTracking()
                 .Where(b => b.Id == bookingId)
                 .Select(b => b.TotalAmount)
                 .FirstOrDefaultAsync();

            if (totalAmount == default(decimal) && !await this.bookingReposotory.AllAsNoTracking().AnyAsync(b => b.Id == bookingId))
            {
                throw new InvalidOperationException($"No booking found with ID {bookingId}.");
            }

            return totalAmount;
        }

        public async Task SavePaymentAndConfirmBooking(Guid bookingId, Payment payment)
        {
            await this.paymentReposiotry.AddAsync(payment);
            await this.propertyReposiotry.SaveChangesAsync();

            var booking = await this.bookingReposotory.All()
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking != null)
            {
                booking.PaymentId = payment.Id;
                booking.IsConfirmed = true;

                await this.bookingReposotory.SaveChangesAsync();
            }
        }

        public async Task<bool> CancelBookingAsync(Guid bookingId, Guid userId)
        {
            var booking = await this.bookingReposotory.All()
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.TenantId == userId);

            if (booking == null) 
            {
                return false;
            }

            this.bookingReposotory.Delete(booking);
            await this.bookingReposotory.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsConfirmed(Guid bookingId)
        {
            return await this.bookingReposotory.AllAsNoTracking()
                .AnyAsync(b => b.Id == bookingId && b.IsConfirmed);
        }
    }
}
