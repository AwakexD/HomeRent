using HomeRent.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Data.Seeding
{
    public class AmenitiesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Amenities.AnyAsync())
            {
                return;
            }

            await AmenitiesSeederAsync(dbContext);
        }

        private static async Task AmenitiesSeederAsync(ApplicationDbContext dbContext)
        {
            var amenities = new HashSet<Amenity>();

            var data = new Dictionary<string, string>()
            {
                {"Оптичен интернет", "bi-wifi"},
                {"Смарт телевизор", "bi-tv"},
                {"Климатик", "bi-snow"},
                {"Подово отопление", "bi-thermometer-high"},
                {"Бойлер", "bi-droplet"},
                {"Оборудвана кухня", "bi-utensils"},
                {"Машина за кафе", "bi-cup-hot"},
                {"Работно пространство", "bi-laptop"},
                {"Пречиствател на въздуха", "bi-wind"},
                {"Шумоизолация", "bi-ear"},
                {"Балкон или тераса", "bi-house-door"},
                {"Барбекю зона", "bi-fire"},
                {"Сейф", "bi-shield-lock"},
                {"Пералня и сушилня", "bi-house-gear"},
                {"Прахосмукачка", "bi-cloud-haze"},
                {"Дъска за гладене", "bi-border-style"},
                {"Паркомясто или гараж", "bi-car-front"},
                {"Допускане на домашни любимци", "bi-paw"},
                {"Хавлии и кърпи", "bi-tags"},
                {"Хладилник", "bi-box"},
                {"Басейн", "bi-water"}
            };


            foreach (var row in data)
            {
                amenities.Add(new Amenity() { Name = row.Key, IconClass = row.Value });
            }

            await dbContext.Amenities.AddRangeAsync(amenities);
        } 
    }
}
