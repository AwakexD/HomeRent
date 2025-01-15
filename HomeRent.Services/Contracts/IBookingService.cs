namespace HomeRent.Services.Contracts
{
    public interface IBookingService
    {
        Task<decimal> GetPropertyPriceAsync(Guid propertyId);
    }
}
