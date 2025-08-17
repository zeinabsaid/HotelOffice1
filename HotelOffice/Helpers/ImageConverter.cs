using System;
using System.IO;

namespace HotelOffice.Helpers
{
    public static class ImageConverter
    {
        public static string ConvertFilePathToDataUrl(string? filePath)
        {
            // مسار الصورة الافتراضية داخل wwwroot
            string placeholder = "images/placeholder.png";

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return placeholder;
            }

            try
            {
                byte[] imageBytes = File.ReadAllBytes(filePath);
                string base64String = Convert.ToBase64String(imageBytes);
                return $"data:image/png;base64,{base64String}";
            }
            catch (Exception)
            {
                // في حالة حدوث أي خطأ، نرجع للصورة الافتراضية
                return placeholder;
            }
        }
    }
}