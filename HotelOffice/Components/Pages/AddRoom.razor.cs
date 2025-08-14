using HotelOffice.Business.Services;
using HotelOffice.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms; // ==> مهم لرفع الملفات
using System.IO; // ==> مهم للتعامل مع الملفات

namespace HotelOffice.Components.Pages
{
    public partial class AddRoom
    {
        [Inject]
        private IRoomService RoomService { get; set; } = null!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        private Room newRoom = new Room();
        private string errorMessage = string.Empty;
        private bool isSaving = false;

        // ==> المتغيرات الجديدة لمعالجة الصور
        private IBrowserFile? selectedFile;
        private string? imageDataUrl;

        // دالة يتم استدعاؤها عند اختيار ملف
        private async Task LoadImage(InputFileChangeEventArgs e)
        {
            selectedFile = e.File;
            if (selectedFile != null)
            {
                // تحويل الصورة إلى رابط بيانات لعرضها (اختياري)
                var format = "image/png";
                var resizedImageFile = await selectedFile.RequestImageFileAsync(format, 1024, 1024);
                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream().ReadAsync(buffer);
                imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                StateHasChanged();
            }
        }

        private async Task HandleAddRoom()
        {
            isSaving = true;
            try
            {
                if (selectedFile != null)
                {
                    // 1. تحديد مسار الحفظ (داخل wwwroot/images/rooms)
                    // تأكد من وجود مجلد 'images' و 'rooms' داخل wwwroot في مشروعك الرئيسي
                    var uploadsFolder = Path.Combine(FileSystem.AppDataDirectory, "images", "rooms");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // 2. إنشاء اسم فريد للملف
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(selectedFile.Name)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // 3. حفظ الملف في المسار
                    await using var fs = new FileStream(filePath, FileMode.Create);
                    await selectedFile.OpenReadStream(maxAllowedSize: 5242880).CopyToAsync(fs); // حد أقصى 5 ميجا

                    // 4. حفظ رابط الصورة في الموديل
                    // سنحفظ رابطًا نسبيًا يمكن للتطبيق استخدامه
                    newRoom.ImageUrl = filePath;
                }

                await RoomService.CreateAsync(newRoom);
                NavigationManager.NavigateTo("/rooms");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                isSaving = false;
                StateHasChanged();
            }
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/rooms");
        }
    }
}