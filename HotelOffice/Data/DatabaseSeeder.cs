using HotelOffice.Business.Interfaces;
using HotelOffice.Data;
using HotelOffice.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelOffice.Data
{
    public static class DatabaseSeeder
    {
        // هذه الدالة ستقوم بكل العمل
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            // نحصل على الخدمات التي نحتاجها (قاعدة البيانات وخدمة المستخدمين)
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                // تأكد من أن قاعدة البيانات قد تم إنشاؤها وتطبيق كل الهجرات
                await dbContext.Database.MigrateAsync();

                // ==> أهم خطوة: نفحص إذا كان هناك أي مستخدم في قاعدة البيانات
                if (!await dbContext.Users.AnyAsync())
                {
                    // إذا لم يكن هناك أي مستخدم، نقوم بإنشاء المدير
                    var adminUser = new User
                    {
                        FullName = "Administrator",
                        Username = "admin",
                        // نضع كلمة المرور كنص عادي هنا، لأن خدمة CreateAsync هي المسؤولة عن تجزئتها (Hashing)
                        PasswordHash = "admin123",
                        Role = "Admin"
                    };

                    await userService.CreateAsync(adminUser);
                }
            }
        }
    }
}