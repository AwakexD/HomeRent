using HomeRent.Models.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeRent.Services.Contracts
{
    public interface IPropertyStaticDataService
    {
        Task<SelectList> GetPropertyTypesSelectList(int? selectedId = null);

        Task<IEnumerable<AmenityViewModel>> GetAmenitiesSelectList(List<int> selectedAmenityIds = null);
    }
}
