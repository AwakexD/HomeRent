using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Services
{
    public class BookingService : IBookingService
    {
        private readonly IDeletableEntityRepository<Property> propertyReposiotry;
        public BookingService(IDeletableEntityRepository<Property> propertyReposiotry)
        {
            this.propertyReposiotry = propertyReposiotry;
        }

        public async Task<decimal> GetPropertyPriceAsync(Guid propertyId)
        {
            return await this.propertyReposiotry.AllAsNoTracking()
                .Where(p => p.Id == propertyId)
                .Select(p => p.PricePerNight)
                .FirstOrDefaultAsync();
        } 
    }
}
