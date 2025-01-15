using AutoMapper;
using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.Shared;
using HomeRent.Models.ViewModels.Dashboard;
using HomeRent.Models.ViewModels.Property;
using HomeRent.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IMapper mapper;

        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<Review> reviewRepository;
        public DashboardService(IMapper mapper, 
            IDeletableEntityRepository<Property> propertyRepository,
            IDeletableEntityRepository<Review> reviewRepository)
        {
            this.mapper = mapper;
            this.propertyRepository = propertyRepository;
            this.reviewRepository = reviewRepository;
        }

        public async Task<OwnerDashboardViewModel> GetOwenerDashboardVM(Guid userId)
        {
            var properties = await this.propertyRepository.AllAsNoTracking()
                .Where(p => p.OwnerId == userId)
                .Select(p => p.Id)
                .ToListAsync();

            int listingsCount = properties.Count();

            // ToDO : Retrieve rented properties
            // Bookings not working yet.
            int rentedPropeties = 2;

            double averageRating = 0;

            if (properties.Any())
            {
                averageRating = await this.reviewRepository.AllAsNoTracking()
                    .Where(r => properties.Contains(r.PropertyId))
                    .AverageAsync(r => r.Rating);
            }

            var viewModel = new OwnerDashboardViewModel
            {
                ListingsCount = listingsCount,
                RentedProperties = rentedPropeties,
                AverageReviewRating = averageRating,
                Properties = await GetOwnerProperties(userId)
            };

            return viewModel;
        }

        public async Task<TenantDashboardViewModel> GetTenantDashboardVM(Guid userId)
        {
            // ToDO : Complete method
            int bookingsCount = 0;
            int favoritesCount = 0;
            int reviewsCount = await this.reviewRepository.AllAsNoTracking()
                .Where(r => r.TenantId == userId)
                .CountAsync();

            var viewModel = new TenantDashboardViewModel
            {
                BookingsCount = bookingsCount,
                FavoritesCount = favoritesCount,
                ReviewsCount = reviewsCount,
            };

            return viewModel;
        }

        private async Task<IEnumerable<SinglePropertyViewModel>> GetOwnerProperties(Guid userId)
        {
            var properties = await this.propertyRepository.AllAsNoTrackingWithDeleted()
                .Include(p => p.Images)
                .Where(p => p.OwnerId == userId)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<SinglePropertyViewModel>>(properties);
        }
    }
}
