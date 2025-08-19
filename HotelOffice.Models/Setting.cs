using System.ComponentModel.DataAnnotations;

namespace HotelOffice.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يجب تحديد وقت الدخول")]
        public TimeOnly CheckInTime { get; set; } = new TimeOnly(14, 0); // قيمة افتراضية: 2:00 PM

        [Required(ErrorMessage = "يجب تحديد وقت الخروج")]
        public TimeOnly CheckOutTime { get; set; } = new TimeOnly(12, 0); // قيمة افتراضية: 12:00 PM
    }
}