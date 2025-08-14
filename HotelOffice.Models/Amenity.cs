using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelOffice.Models
{
    public class Amenity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // ==> علاقة: كل مرفق يمكن أن يكون في غرف كثيرة
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
