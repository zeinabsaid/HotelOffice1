using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelOffice.Models
{
    public class Guest
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم النزيل مطلوب")]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        // ==> الخطوة 1: أضيفي هذا السطر هنا
        public string? NationalId { get; set; }

        public string? IdCardImageUrl { get; set; }
    }
}