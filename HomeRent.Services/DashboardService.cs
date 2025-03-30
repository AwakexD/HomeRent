using AutoMapper;
using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.Shared;
using HomeRent.Models.ViewModels.Booking;
using HomeRent.Models.ViewModels.Dashboard;
using HomeRent.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IMapper mapper;

        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<Review> reviewRepository;
        private readonly IDeletableEntityRepository<Booking> bookingRepository;
        public DashboardService(IMapper mapper, 
            IDeletableEntityRepository<Property> propertyRepository,
            IDeletableEntityRepository<Review> reviewRepository,
            IDeletableEntityRepository<Booking> bookingRepository)
        {
            this.mapper = mapper;
            this.propertyRepository = propertyRepository;
            this.reviewRepository = reviewRepository;
            this.bookingRepository = bookingRepository;
        }

        public async Task<OwnerDashboardViewModel> GetOwnerDashboardVm(Guid userId)
        {
            var properties = await this.propertyRepository.AllAsNoTracking()
                .Where(p => p.OwnerId == userId)
                .Select(p => p.Id)
                .ToListAsync();

            int listingsCount = properties.Count();

            var rentedProperties = await this.bookingRepository.AllAsNoTracking()
                .Where(b => b.Property.OwnerId == userId)
                .CountAsync();

            double averageRating = 0;

            if (properties.Any())
            {
                var ratings = await this.reviewRepository.AllAsNoTracking()
                    .Where(r => properties.Contains(r.PropertyId))
                    .Select(r => r.Rating)
                    .ToListAsync();

                averageRating = ratings.Any() ? ratings.Average() : 0;
            }

            var viewModel = new OwnerDashboardViewModel
            {
                ListingsCount = listingsCount,
                RentedProperties = rentedProperties,
                AverageReviewRating = averageRating,
                Properties = await GetOwnerProperties(userId)
            };

            return viewModel;
        }

        public async Task<TenantDashboardViewModel> GetTenantDashboardVm(Guid userId)
        {
            int bookingsCount = await this.bookingRepository.AllAsNoTracking()
                .Where(b => b.TenantId == userId)
                .CountAsync();
            
            int reviewsCount = await this.reviewRepository.AllAsNoTracking()
                .Where(r => r.TenantId == userId)
                .CountAsync();

            var viewModel = new TenantDashboardViewModel
            {
                BookingsCount = bookingsCount,
                ReviewsCount = reviewsCount,
                Bookings = await this.GetTenantBookings(userId)
            };

            return viewModel;
        }

        public async Task<IEnumerable<BookingTableViewModel>> GetOwnerBookings(Guid userId)
        {
            var bookings = await this.bookingRepository.AllAsNoTrackingWithDeleted()
                .Where(b => b.Property.OwnerId == userId)
                .Include(b => b.Property)
                .Include(b => b.Property.Images.Where(i => i.IsDeleted == false))
                .Include(b => b.Tenant)
                .Include(b => b.Payment)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<BookingTableViewModel>>(bookings);
        }

        private async Task<IEnumerable<SinglePropertyViewModel>> GetOwnerProperties(Guid userId)
        {
            var properties = await this.propertyRepository.AllAsNoTrackingWithDeleted()
                .Include(p => p.Images.Where(i => i.IsDeleted == false))
                .Where(p => p.OwnerId == userId)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<SinglePropertyViewModel>>(properties);
        }

        private async Task<IEnumerable<BookingTableViewModel>> GetTenantBookings(Guid userId)
        {
            var bookings = await this.bookingRepository.AllAsNoTrackingWithDeleted()
                .Include(b => b.Property)
                .Include(b => b.Property.Owner)
                .Include(b => b.Property.Images.Where(i => i.IsDeleted == false))
                .ToListAsync();

            return this.mapper.Map<IEnumerable<BookingTableViewModel>>(bookings);
        }
    }
}
