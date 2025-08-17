using HotelOffice.Business.Interfaces;
using HotelOffice.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelOffice.Components.Pages
{
    public class AmenitiesManagementBase : ComponentBase
    {
        [Inject]
        private IAmenityService AmenityService { get; set; } = default!;

        protected List<Amenity>? amenities;
        protected Amenity currentAmenity = new();
        protected bool isEditing = false;

        // ✅ متغيرات جديدة لإدارة الأخطاء وتأكيد الحذف
        protected string? errorMessage;
        protected Amenity? amenityToDelete;

        protected override async Task OnInitializedAsync()
        {
            await LoadAmenitiesAsync();
        }

        private async Task LoadAmenitiesAsync()
        {
            amenities = await AmenityService.GetAllAmenitiesAsync();
        }

        // ✅ تمت إضافة معالجة الأخطاء
        protected async Task HandleSaveAsync()
        {
            if (string.IsNullOrWhiteSpace(currentAmenity.Name)) return;
            errorMessage = null; // مسح أي رسالة خطأ قديمة

            try
            {
                if (isEditing)
                {
                    await AmenityService.UpdateAmenityAsync(currentAmenity);
                }
                else
                {
                    // نفترض أن هذه الدالة تطلق خطأ إذا كان المرفق مكررًا
                    await AmenityService.AddAmenityAsync(currentAmenity);
                }

                currentAmenity = new();
                isEditing = false;
                await LoadAmenitiesAsync();
            }
            catch (System.Exception ex)
            {
                // عرض رسالة الخطأ للمستخدم بدلاً من انهيار الصفحة
                errorMessage = ex.Message;
            }
        }

        protected void StartEdit(Amenity amenityToEdit)
        {
            errorMessage = null;
            currentAmenity = new Amenity { Id = amenityToEdit.Id, Name = amenityToEdit.Name };
            isEditing = true;
        }

        protected void CancelEdit()
        {
            errorMessage = null;
            currentAmenity = new();
            isEditing = false;
        }

        // ✅ خطوات جديدة لتأكيد الحذف
        protected void RequestDeleteConfirmation(Amenity amenity)
        {
            amenityToDelete = amenity;
        }

        protected async Task ConfirmDeleteAsync()
        {
            if (amenityToDelete == null) return;

            await AmenityService.DeleteAmenityAsync(amenityToDelete.Id);
            amenityToDelete = null; // لإخفاء النافذة
            await LoadAmenitiesAsync();
        }

        protected void CancelDelete()
        {
            amenityToDelete = null;
        }
    }
}