using HotelOffice.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HotelOffice.Business.Services;
using System.Threading.Tasks;
using HotelOffice.ViewModels;
using HotelOffice.Business.Interfaces;

namespace HotelOffice
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "HotelOffice.db");

            // --- ✅  هذا هو التعديل الأهم: استخدام AddDbContext ---
            // هذا يسجل DbContext نفسه كخدمة Scoped
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}")
            );

            // 3. تسجيل خدمات Business Logic
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IGuestService, GuestService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
            builder.Services.AddScoped<IFloorService, FloorService>();
            builder.Services.AddScoped<IAmenityService, AmenityService>();
            builder.Services.AddScoped<IHotelSettingsService, HotelSettingsService>();
            // تسجيل ViewModels
            builder.Services.AddTransient<ReceptionistDashboardViewModel>();
            builder.Services.AddTransient<NewBookingViewModel>();
            builder.Services.AddTransient<BookingDetailsViewModel>();

            var app = builder.Build();

            // استدعاء المساعد لتهيئة قاعدة البيانات عند بدء التشغيل
            _ = Task.Run(() => DatabaseHelper.InitializeDatabaseAsync(app.Services));

            try
            {
                Task.Run(() => HotelOffice.Data.DatabaseSeeder.SeedAsync(app.Services)).Wait();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred during database seeding: {ex.Message}");
            }

            return app;
        }
    }
}