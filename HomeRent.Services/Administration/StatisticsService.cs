using HomeRent.Data.Models.Entities;
using HomeRent.Data.Models.User;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.Administration;
using HomeRent.Services.Administration.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Services.Administration
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<Booking> bookingRepository;
        private readonly IDeletableEntityRepository<Review> reviewRepository;
        private readonly IDeletableEntityRepository<Payment> paymentRepository;

        public StatisticsService(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<Property> propertyRepository,
            IDeletableEntityRepository<Booking> bookingRepository,
            IDeletableEntityRepository<Review> reviewRepository,
            IDeletableEntityRepository<Payment> paymentRepository)
        {
            this.userRepository = userRepository;
            this.propertyRepository = propertyRepository;
            this.bookingRepository = bookingRepository;
            this.reviewRepository = reviewRepository;
            this.paymentRepository = paymentRepository;
        }

        public async Task<AdminStatisticsViewModel> GetAdminStatisticsAsync()
        {
            var totalUsers = await userRepository.AllAsNoTracking()
                .CountAsync();

            var totalActiveProperties = await propertyRepository.AllAsNoTracking()
                .CountAsync(p => !p.IsDeleted);

            var totalBookings = await bookingRepository.AllAsNoTracking()
                .CountAsync(b => !b.IsDeleted);

            var totalReviews = await reviewRepository.AllAsNoTracking()
                .CountAsync(r => !r.IsDeleted);

            var totalRevenue = await paymentRepository.AllAsNoTracking()
                .Where(p => !p.IsDeleted && p.Status == "Succeeded")
                .SumAsync(p => (decimal?)p.AmountPaid ?? 0);

            var reservationQuery = await bookingRepository.AllAsNoTracking()
                .Where(b => !b.IsDeleted)
                .Select(b => (decimal?)b.TotalAmount)
                .ToListAsync();

            var avgReservationPrice = reservationQuery.Any()
                ? reservationQuery.Average() ?? 0
                : 0;

            return new AdminStatisticsViewModel
            {
                TotalUsers = totalUsers,
                TotalActiveProperties = totalActiveProperties,
                TotalBookings = totalBookings,
                TotalReviews = totalReviews,
                TotalRevenue = totalRevenue,
                AverageReservationPrice = avgReservationPrice,
            };
        }
    }
}
