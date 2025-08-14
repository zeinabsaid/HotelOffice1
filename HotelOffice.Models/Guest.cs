using System.ComponentModel.DataAnnotations;

namespace HotelOffice.Models
{
    public class Guest
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم النزيل مطلوب")]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(50)]
        public string? NationalId { get; set; }
     

        [MaxLength(250)]
        public string? Address { get; set; }

        public string? IdCardImageUrl { get; set; }
    }
}