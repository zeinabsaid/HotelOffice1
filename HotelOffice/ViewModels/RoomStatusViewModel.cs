using HotelOffice.Models;
using Microsoft.Maui.Graphics;

namespace HotelOffice.ViewModels
{
    // هذا الكلاس يمثل بيانات الغرفة الواحدة التي ستُعرض في الواجهة
    public class RoomStatusViewModel
    {
        public required Room Room { get; set; }
        public required string StatusText { get; set; }
        public required Color StatusColor { get; set; }
        public Booking? ActiveBooking { get; set; } // الحجز الحالي أو القادم للغرفة

        // خصائص مساعدة للتحكم في ظهور الأزرار في الواجهة
        public bool IsAvailable => ActiveBooking == null;
        public bool CanCheckIn => ActiveBooking?.BookingStatus == "Confirmed";
        public bool CanCheckOut => ActiveBooking?.BookingStatus == "CheckedIn";
    }
}