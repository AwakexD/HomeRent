namespace HomeRent.Models.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public bool IsOwner { get; set; }

        public bool IsTenant { get; set; }

        public OwnerDashboardViewModel OwnerDashboard { get; set; }

        public TenantDashboardViewModel TenantDashboard { get; set; }
    }
}
