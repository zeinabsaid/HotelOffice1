// ✅ تم تحديث الكود ليعمل مع الموديل الجديد "Setting"
using HotelOffice.Models;
using System.Threading.Tasks;

namespace HotelOffice.Business.Interfaces
{
    public interface IHotelSettingsService
    {
        Task<Setting> GetSettingsAsync();
        Task SaveSettingsAsync(Setting settings);
    }
}