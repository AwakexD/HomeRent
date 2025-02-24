using HomeRent.Data;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Data.Models.User;
using HomeRent.Data.Repositories;
using HomeRent.Data.Infrastructure;
using HomeRent.Data.Seeding;
using HomeRent.Services;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CloudinaryDotNet;
using Ganss.Xss;
using Stripe;
using System.Globalization;
using HomeRent.Models.Validation;
using HomeRent.Services.Administration;
using HomeRent.Services.Administration.Contracts;
using Microsoft.AspNetCore.Localization;
using HomeRent.Web.Hubs;

namespace HomeRent.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");
            ConfigureServices(builder.Services, builder.Configuration);

            builder.Services.AddAutoMapper(typeof(Program));

            // Set default culture to Bulgarian (bg-BG)
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("bg-BG");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("bg-BG");
            var supportedCultures = new[] { new CultureInfo("bg-BG") };

            var app = builder.Build();
            Configure(app);
            app.UseAuthentication();
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("bg-BG"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    options.ModelBinderProviders.Insert(0, new CustomDecimalModelBinderProvider());
                }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSignalR();

            services.AddSingleton(configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IPropertyStaticDataService, PropertyStaticDataService>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IReviewService, HomeRent.Services.ReviewService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<IStripeService, StripeService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDataManagementService, DataManagementService>();

            // Cloudinary
            Cloudinary cloudinary = new Cloudinary(configuration["Cloudinary:CLOUDINARY_URL"]);
            cloudinary.Api.Secure = true;
            services.AddSingleton(cloudinary);

            // HTML Sanitizer
            services.AddSingleton<IHtmlSanitizer, HtmlSanitizer>();
        }

        private static void Configure(WebApplication app)
        {
            // Seed data on application startup
            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<UserStatusHub>("/userStatusHub");
            });
            app.MapRazorPages();
        }
    }
}