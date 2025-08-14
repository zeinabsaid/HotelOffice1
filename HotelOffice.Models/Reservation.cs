using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelOffice.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        // --- ربط الجداول ---
        [Required]
        public int GuestId { get; set; }
        [ForeignKey("GuestId")]
        public virtual Guest Guest { get; set; } = null!;

        [Required]
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; } = null!;

        // --- تفاصيل التواريخ ---
        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        // --- التفاصيل المالية ---
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal AmountPaid { get; set; }

        [NotMapped] // هذا الحقل سيتم حسابه ولن يتم تخزينه في قاعدة البيانات
        public decimal RemainingBalance => TotalAmount - AmountPaid;

        // --- حالة الحجز ---
        [Required]
        public ReservationStatus Status { get; set; }

        public string? Notes { get; set; }
    }

    // هذا الـ "enum" يحدد الحالات الممكنة للحجز لتجنب الأخطاء
    public enum ReservationStatus
    {
        Confirmed,  // مؤكد
        CheckedIn,  // تم تسجيل الدخول
        CheckedOut, // تم تسجيل المغادرة
        Cancelled   // ملغي
    }
}