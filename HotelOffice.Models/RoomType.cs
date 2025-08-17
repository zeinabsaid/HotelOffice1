// In HotelOffice.Models/RoomType.cs

using System.ComponentModel.DataAnnotations;

namespace HotelOffice.Models
{
    public class RoomType
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "نوع الغرفة مطلوب")]
        [Display(Name = "نوع الغرفة")]
        public string Name { get; set; } = string.Empty;

        // علاقة One-to-Many: نوع الغرفة الواحد يمكن أن يتواجد في العديد من الغرف
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}