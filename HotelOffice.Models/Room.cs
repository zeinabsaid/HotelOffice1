using System.ComponentModel.DataAnnotations;

namespace HotelOffice.Models
{

    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "رقم الغرفة مطلوب")]
        public string RoomNumber { get; set; } = string.Empty;

        public string? RoomType { get; set; }

        [Range(1, 10000, ErrorMessage = "السعر يجب أن يكون قيمة موجبة")]
        public decimal PricePerNight { get; set; }

        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

        // ==> علاقة جديدة: كل غرفة لها حجوزات متعددة
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        // ==> المتطلب الجديد: علاقة Many-to-Many مع المرافق
        public virtual ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
    }
}
