using HotelOffice.Business.Interfaces;
using HotelOffice.Data;
using HotelOffice.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HotelOffice.Business.Services
{
    public class HotelSettingsService : IHotelSettingsService
    {
        private readonly ApplicationDbContext _db;

        public HotelSettingsService(ApplicationDbContext context)
        {
            _db = context;
        }

        // هذه الدالة تم تحديثها في الرد السابق وهي سليمة
        public async Task<Setting> GetSettingsAsync()
        {
            var settings = await _db.Settings.FirstOrDefaultAsync();

            if (settings == null)
            {
                // ✅  نقوم بإنشاء الإعدادات الافتراضية باستخدام TimeOnly
                settings = new Setting
                {
                    CheckInTime = new TimeOnly(14, 0), // 2:00 PM
                    CheckOutTime = new TimeOnly(12, 0)  // 12:00 PM
                };
                _db.Settings.Add(settings);
                await _db.SaveChangesAsync();
            }

            return settings;
        }

        // ✅  هذه هي الدالة التي كانت تسبب الخطأ، وقد تم تصحيحها الآن
        // تم تغيير نوع المتغير من HotelSettings إلى Setting
        public async Task SaveSettingsAsync(Setting settings)
        {
            var settingsFromDb = await _db.Settings.FindAsync(settings.Id);

            if (settingsFromDb != null)
            {
                _db.Entry(settingsFromDb).CurrentValues.SetValues(settings);
            }
            else
            {
                _db.Settings.Add(settings);
            }

            await _db.SaveChangesAsync();
        }
    }
}