using HotelOffice.Models;
using System.Linq.Expressions;

namespace HotelOffice.Business.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllAsync(
            Expression<Func<Booking, bool>>? filter = null,
            string? includeProperties = null
        );

        Task<Booking?> GetByIdAsync(int id);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate, int? excludeBookingId = null);
        Task<Booking> CreateAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(int id);
        Task<bool> RecordPaymentAsync(int bookingId, decimal amount);
        Task<bool> UpdateBookingStatusAsync(int bookingId, string newStatus);

        // ==> الدالة الجديدة لحساب الإيرادات
        Task<decimal> CalculateRevenueAsync(DateTime startDate, DateTime endDate);
    }
}