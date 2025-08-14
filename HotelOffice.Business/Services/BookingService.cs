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

        // دالة التحقق من توافر الغرفة - أهم جزء في النظام
        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate, int? excludeBookingId = null)
        {
            // بناء الاستعلام الأولي للبحث عن الحجوزات المتداخلة
            var overlappingBookingsQuery = _db.Bookings
                .Where(b => b.RoomId == roomId &&
                            b.BookingStatus != "Cancelled" && // لا نحسب الحجوزات الملغاة
                            b.CheckInDate < checkOutDate &&
                            b.CheckOutDate > checkInDate);

            // إذا كنا نقوم بتحديث حجز موجود، يجب أن نستثنيه من عملية التحقق
            if (excludeBookingId.HasValue)
            {
                overlappingBookingsQuery = overlappingBookingsQuery.Where(b => b.Id != excludeBookingId.Value);
            }

            // إذا كان عدد الحجوزات المتداخلة صفرًا، فالغرفة متاحة
            var isAvailable = !await overlappingBookingsQuery.AnyAsync();
            return isAvailable;
        }

        public async Task<Booking> CreateAsync(Booking booking)
        {
            // 1. التحقق من التوافر قبل أي شيء
            var isAvailable = await IsRoomAvailableAsync(booking.RoomId, booking.CheckInDate, booking.CheckOutDate);
            if (!isAvailable)
            {
                // إذا كانت الغرفة غير متاحة، نطلق خطأ لنمنع إنشاء الحجز
                throw new InvalidOperationException("This room is not available for the selected dates.");
            }

            // 2. حساب عدد الليالي والتكلفة الإجمالية
            var room = await _db.Rooms.FindAsync(booking.RoomId);
            if (room != null)
            {
                booking.NumberOfNights = (booking.CheckOutDate - booking.CheckInDate).Days;
                booking.TotalCost = booking.NumberOfNights * room.PricePerNight;
            }

            // 3. إضافة الحجز وحفظه
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

            // ترتيب الحجوزات من الأحدث للأقدم
            return await query.OrderByDescending(b => b.CheckInDate).ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _db.Bookings.FindAsync(id);
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
    }
}