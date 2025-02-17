using HomeRent.Models.Shared;

namespace HomeRent.Models.ViewModels.Dashboard
{
    public class DashboardReviewViewModel : BaseReviewViewModel
    {
        public int Id { get; set; }

        public Guid PropertyId { get; set; }

        public string TenantEmail { get; set; }
    }
}
