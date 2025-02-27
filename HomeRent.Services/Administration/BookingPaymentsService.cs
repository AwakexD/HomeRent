using AutoMapper;
using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.Administration;
using HomeRent.Services.Administration.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Services.Administration
{
    public class BookingPaymentsService : IBookingPaymentsService
    {
        private readonly IMapper mapper;

        private readonly IDeletableEntityRepository<Booking> bookingsRepository;

        public BookingPaymentsService(IMapper mapper,
            IDeletableEntityRepository<Booking> bookingsRepository)
        {
            this.mapper = mapper;
            this.bookingsRepository = bookingsRepository;
        }

        public async Task<IEnumerable<BookingPaymentViewModel>> GetAllBookingsAndPaymentsAsync()
        {
            var bookings = await this.bookingsRepository.AllAsNoTrackingWithDeleted()
                .Include(b => b.Tenant)
                .Include(b => b.Payment)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<BookingPaymentViewModel>>(bookings);
        }
    }
}
