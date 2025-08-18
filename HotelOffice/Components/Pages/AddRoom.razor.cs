using HotelOffice.Business.Interfaces;
using HotelOffice.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HotelOffice.Components.Pages
{
    public partial class AddRoomBase : ComponentBase
    {
        [Inject] protected IRoomService RoomService { get; set; } = default!;
        [Inject] protected IRoomTypeService RoomTypeService { get; set; } = default!;
        [Inject] protected IFloorService FloorService { get; set; } = default!;
        [Inject] protected IAmenityService AmenityService { get; set; } = default!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

        protected Room newRoom = new Room();
        protected string? errorMessage;
        protected bool isSaving = false;
        protected bool IsLoading = true;

        protected List<RoomType> roomTypes = new();
        protected List<Floor> floors = new();
        protected string? imageDataUrl;

        // --- ✅  منطق جديد للمرافق ---
        protected bool IsAmenitiesModalOpen = false;
        protected List<Amenity> allAmenities = new();
        protected HashSet<int> selectedAmenityIds = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // لم نعد نحتاج إلى تحميل المرافق هنا في البداية
                roomTypes = await RoomTypeService.GetAllRoomTypesAsync() ?? new List<RoomType>();
                floors = await FloorService.GetAllFloorsAsync() ?? new List<Floor>();
            }
            catch (Exception ex)
            {
                errorMessage = $"حدث خطأ فادح أثناء تحميل البيانات: {ex.ToString()}";
                Console.WriteLine(errorMessage);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // --- ✅  دوال جديدة لإدارة النافذة المنبثقة ---
        protected async Task OpenAmenitiesModal()
        {
            // تحميل المرافق فقط عند الحاجة إليها
            if (!allAmenities.Any())
            {
                allAmenities = await AmenityService.GetAllAmenitiesAsync() ?? new List<Amenity>();
            }
            IsAmenitiesModalOpen = true;
        }

        protected void CloseAmenitiesModal()
        {
            IsAmenitiesModalOpen = false;
        }

        protected void ToggleAmenitySelection(int amenityId)
        {
            if (selectedAmenityIds.Contains(amenityId))
            {
                selectedAmenityIds.Remove(amenityId);
            }
            else
            {
                selectedAmenityIds.Add(amenityId);
            }
        }

        protected async Task HandleAddRoom()
        {
            // --- ✅  هذا هو السطر المضاف للتشخيص ---
            Console.WriteLine("!!!!!! زر الحفظ تم الضغط عليه والتحقق نجح !!!!!!");

            isSaving = true;
            errorMessage = null;
            try
            {
                // بناء قائمة المرافق من الـ IDs المختارة
                newRoom.Amenities = allAmenities.Where(a => selectedAmenityIds.Contains(a.Id)).ToList();

                await RoomService.CreateAsync(newRoom);
                NavigationManager.NavigateTo("/rooms");
            }
            catch (Exception ex)
            {
                errorMessage = $"خطأ أثناء حفظ الغرفة: {ex.Message}";
            }
            finally
            {
                isSaving = false;
            }
        }
        protected async Task HandleImageUpload(InputFileChangeEventArgs e)
        {
            errorMessage = null;
            var file = e.File;
            if (file == null) return;

            var maxFileSize = 10 * 1024 * 1024;
            if (file.Size > maxFileSize)
            {
                errorMessage = "حجم الملف كبير جدًا. الرجاء اختيار صورة أصغر من 10 ميجابايت.";
                return;
            }
            try
            {
                var resizedImageFile = await file.RequestImageFileAsync("image/jpeg", 600, 600);
                using var stream = resizedImageFile.OpenReadStream(maxFileSize);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                newRoom.ImageData = imageBytes;
                imageDataUrl = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
            }
            catch (Exception ex)
            {
                errorMessage = $"حدث خطأ أثناء معالجة الصورة: {ex.Message}";
                Console.WriteLine(ex);
            }
        }

        protected void Cancel()
        {
            NavigationManager.NavigateTo("/rooms");
        }
    }
}