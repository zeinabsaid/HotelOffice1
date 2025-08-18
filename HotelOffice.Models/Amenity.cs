using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelOffice.Models
{
    public class Amenity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المرفق مطلوب")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // --- التعريف الصحيح: كل مرفق يمكن أن يكون في غرف كثيرة ---
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}