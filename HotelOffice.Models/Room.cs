using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelOffice.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "رقم الغرفة مطلوب")]
        public string RoomNumber { get; set; } = string.Empty;

        // ===================================================================
        // >> الحقول الجديدة التي تمت إضافتها <<
        // ===================================================================
        [Range(1, 10, ErrorMessage = "يجب أن يكون عدد الأشخاص بين 1 و 10")]
        [Display(Name = "عدد الأشخاص")]
        public int Capacity { get; set; } = 1; // قيمة افتراضية

        [Range(1, 5, ErrorMessage = "يجب أن يكون عدد الأسرّة بين 1 و 5")]
        [Display(Name = "عدد الأسرّة")]
        public int NumberOfBeds { get; set; } = 1; // قيمة افتراضية
        // ===================================================================

        [Range(1, 10000, ErrorMessage = "السعر يجب أن يكون قيمة موجبة")]
        public decimal PricePerNight { get; set; }

        public byte[]? ImageData { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "يجب تحديد الطابق")]
        public int FloorId { get; set; }

        [Required(ErrorMessage = "يجب تحديد نوع الغرفة")]
        public int RoomTypeId { get; set; }

        [ForeignKey("FloorId")]
        public virtual Floor? Floor { get; set; }

        [ForeignKey("RoomTypeId")]
        public virtual RoomType? RoomType { get; set; } 

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
    }
}