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
        protected List<Amenity> allAmenities = new();
        protected Dictionary<int, bool> selectedAmenities = new();
        protected string? imageDataUrl;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // التحميل بالتتابع هو الطريقة الأكثر أمانًا لمنع تضارب DbContext
                roomTypes = await RoomTypeService.GetAllRoomTypesAsync() ?? new List<RoomType>();
                floors = await FloorService.GetAllFloorsAsync() ?? new List<Floor>();
                allAmenities = await AmenityService.GetAllAmenitiesAsync() ?? new List<Amenity>();

                if (allAmenities != null)
                {
                    // التحصين ضد أي بيانات مكررة قد تأتي من قاعدة البيانات
                    selectedAmenities = allAmenities
                        .GroupBy(a => a.Id)
                        .Select(g => g.First())
                        .ToDictionary(a => a.Id, a => false);
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"حدث خطأ فادح أثناء تحميل البيانات: {ex.ToString()}";
                Console.WriteLine(errorMessage); // لطباعة الخطأ الكامل في نافذة الـ Output
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected async Task HandleAddRoom()
        {
            isSaving = true;
            errorMessage = null;
            try
            {
                newRoom.Amenities.Clear();
                var selectedAmenityIds = selectedAmenities.Where(kv => kv.Value).Select(kv => kv.Key).ToHashSet();

                // التأكد من عدم إضافة مرافق مكررة عند الحفظ
                newRoom.Amenities = allAmenities.Where(a => selectedAmenityIds.Contains(a.Id)).DistinctBy(a => a.Id).ToList();

                await RoomService.CreateAsync(newRoom);
                NavigationManager.NavigateTo("/rooms");
            }
            catch (Exception ex)
            {
                errorMessage = $"خطأ أثناء حفظ الغرفة: {ex.Message}";
                Console.WriteLine(ex);
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

            var maxFileSize = 10 * 1024 * 1024; // 10 MB
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