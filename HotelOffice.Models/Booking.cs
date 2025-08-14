using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelOffice.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        // --- مفاتيح الربط ---
        [Required] public int RoomId { get; set; }
        [ForeignKey("RoomId")] public virtual Room Room { get; set; } = null!;

        [Required] public int GuestId { get; set; }
        [ForeignKey("GuestId")] public virtual Guest Guest { get; set; } = null!;

        [Required] public int UserId { get; set; }
        [ForeignKey("UserId")] public virtual User User { get; set; } = null!;

        // --- تفاصيل الحجز ---
        [Required] public DateTime CheckInDate { get; set; }
        [Required] public DateTime CheckOutDate { get; set; }
        public int NumberOfNights { get; set; }
        public decimal TotalCost { get; set; }
        public string? BookingStatus { get; set; } // "Confirmed", "CheckedIn", "CheckedOut", "Cancelled"
        public string? Notes { get; set; }
    }
}
