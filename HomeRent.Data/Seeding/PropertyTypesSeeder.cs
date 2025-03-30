using HomeRent.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Data.Seeding
{
    public class PropertyTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.PropertyTypes.AnyAsync())
            {
                return;
            }

            await SeedPropertyTypesAsync(dbContext);
        }

        private async Task SeedPropertyTypesAsync(ApplicationDbContext dbContext)
        {
            var propertyTypes = new HashSet<PropertyType>();

            var typeNames = new string[] { "Апартамент", "Къща", "Вила", "Мезонет", "Гараж", "Бунгало"};

            foreach (var type in typeNames)
            {
                propertyTypes.Add(new PropertyType() { Name = type });
            }

            await dbContext.PropertyTypes.AddRangeAsync(propertyTypes);
        }
    }
}
