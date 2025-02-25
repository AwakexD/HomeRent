using HomeRent.Models.Shared;
using HomeRent.Models.ViewModels.Property;

namespace HomeRent.Models.Administration
{
    public class PropertiesIndexViewModel
    {
        public IEnumerable<PropertyListItemViewModel> Properties { get; set; }

        public PagingViewModel Paging { get; set; }
    }
}
