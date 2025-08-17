// In HotelOffice.Models/Floor.cs

using System.ComponentModel.DataAnnotations;

namespace HotelOffice.Models
{
    public class Floor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الطابق مطلوب")]
        [Display(Name = "اسم الطابق")]
        public string Name { get; set; } = string.Empty;

        // علاقة One-to-Many: الطابق الواحد يحتوي على العديد من الغرف
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}