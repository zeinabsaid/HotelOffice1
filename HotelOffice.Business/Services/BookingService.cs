using HotelOffice.Business.Interfaces;
using HotelOffice.Data;
using HotelOffice.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelOffice.Business.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _db;

        public BookingService(ApplicationDbContext context)
        {
            _db = context;
        }

        // دالة التحقق من توافر الغرفة
        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate, int? excludeBookingId = null)
        {
            var overlappingBookingsQuery = _db.Bookings
                .Where(b => b.RoomId == roomId &&
                            b.BookingStatus != "Cancelled" &&
                            b.CheckInDate < checkOutDate &&
                            b.CheckOutDate > checkInDate);

            if (excludeBookingId.HasValue)
            {
                overlappingBookingsQuery = overlappingBookingsQuery.Where(b => b.Id != excludeBookingId.Value);
            }

            var isAvailable = !await overlappingBookingsQuery.AnyAsync();
            return isAvailable;
        }

        public async Task<Booking> CreateAsync(Booking booking)
        {
            var isAvailable = await IsRoomAvailableAsync(booking.RoomId, booking.CheckInDate, booking.CheckOutDate);
            if (!isAvailable)
            {
                throw new InvalidOperationException("This room is not available for the selected dates.");
            }

            var room = await _db.Rooms.FindAsync(booking.RoomId);
            if (room != null)
            {
                booking.NumberOfNights = (booking.CheckOutDate - booking.CheckInDate).Days;
                booking.TotalCost = booking.NumberOfNights * room.PricePerNight;
            }

            await _db.Bookings.AddAsync(booking);
            await _db.SaveChangesAsync();
            return booking;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync(Expression<Func<Booking, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Booking> query = _db.Bookings;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            return await query.OrderByDescending(b => b.CheckInDate).ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _db.Bookings
                .Include(b => b.Room)
                .Include(b => b.Guest)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateAsync(Booking booking)
        {
            _db.Bookings.Update(booking);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking != null)
            {
                _db.Bookings.Remove(booking);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool> RecordPaymentAsync(int bookingId, decimal amount)
        {
            var booking = await _db.Bookings.FindAsync(bookingId);
            if (booking == null || amount <= 0) return false;

            booking.AmountPaid += amount;

            if (booking.AmountPaid > booking.TotalCost)
            {
                booking.AmountPaid = booking.TotalCost;
            }

            _db.Bookings.Update(booking);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateBookingStatusAsync(int bookingId, string newStatus)
        {
            var booking = await _db.Bookings.FindAsync(bookingId);
            if (booking == null) return false;

            if (newStatus == "CheckedOut")
            {
                if (booking.RemainingBalance > 0)
                {
                    throw new InvalidOperationException("Cannot check-out with an outstanding balance. Remaining amount is: " + booking.RemainingBalance);
                }
            }

            booking.BookingStatus = newStatus;

            _db.Bookings.Update(booking);
            await _db.SaveChangesAsync();
            return true;
        }

        // ==> التنفيذ الفعلي لدالة حساب الإيرادات
        public async Task<decimal> CalculateRevenueAsync(DateTime startDate, DateTime endDate)
        {
            // سنقوم بتجميع التكلفة الإجمالية للحجوزات التي تم تسجيل مغادرتها خلال الفترة المحددة
            var revenue = await _db.Bookings
                .Where(b => b.BookingStatus == "CheckedOut" &&
                            b.CheckOutDate.Date >= startDate.Date &&
                            b.CheckOutDate.Date <= endDate.Date)
                .SumAsync(b => b.TotalCost);

            return revenue;
        }
    }
}