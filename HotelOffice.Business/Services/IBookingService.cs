using HotelOffice.Models;
using System.Linq.Expressions;

namespace HotelOffice.Business.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllAsync(
            Expression<Func<Booking, bool>>? filter = null,
            string? includeProperties = null // لجلب بيانات الغرفة والنزيل مع الحجز
        );

        Task<Booking?> GetByIdAsync(int id);

        // ==> دالة جديدة وحاسمة للتحقق من توافر الغرفة في فترة معينة
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate, int? excludeBookingId = null);

        // ==> دالة محسنة لإنشاء الحجز
        Task<Booking> CreateAsync(Booking booking);

        Task UpdateAsync(Booking booking);
        Task DeleteAsync(int id);
    }
}