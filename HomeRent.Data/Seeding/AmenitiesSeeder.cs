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
                {"Сешоар", "fi fi-br-scanner-gun"},
                {"Шампоан", "fi fi-rr-soap"},
                {"Сапун", "fi fi-tc-soap"},
                {"Хавлии и кърпи", "fi fi-ts-clothing-rack"},
                {"Тоалетна хартия", "fi fi-rr-toilet-paper-blank"},
                {"Спално бельо", "fi fi-rs-mattress-pillow"},
                {"Място за дрехи", "fi fi-tr-clothing-rack"},
                {"Пералня и сушилня", "fi fi-rs-washer"},
                {"Ютия и дъска за гладене", "fi fi-rs-steam-iron"},
                {"Климатик", "fi fi-rr-snowflakes"},
                {"Отоплителна система", "fi fi-rs-thermometer-three-quarters"},
                {"Вентилатори", "fi fi-rs-fan-table"},
                {"Wi-Fi", "fi fi-br-wifi"},
                {"Работно място", "fi fi-rr-desk"},
                {"Офис консумативи", "fi fi-sr-pencil"},
                {"Напълно оборудвана кухня", "fi fi-rr-hat-chef"},
                {"Хладилник", "fi fi-rr-refrigerator"},
                {"Микровълнова машина", "fi fi-rr-microwave"},
                {"Кафемашина", "fi fi-rr-coffee-pot"},
                {"Съдомиална машина", "fi fi-bs-plate"},
                {"Балкон", "fi fi-ts-balcony"},
                {"Барбекю грил", "fi fi-rr-grill"},
                {"Открита зона за сядане", "fi fi-rs-bench-tree"},
                {"Плувен басейн", "fi fi-br-swimming-pool"},
            };

            foreach (var row in data)
            {
                amenities.Add(new Amenity() { Name = row.Key, IconClass = row.Value });
            }

            await dbContext.Amenities.AddRangeAsync(amenities);
        } 
    }
}
