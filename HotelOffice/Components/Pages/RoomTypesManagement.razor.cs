using HotelOffice.Business.Interfaces;
using HotelOffice.Models;
using Microsoft.AspNetCore.Components;

namespace HotelOffice.Components.Pages
{
    public partial class RoomTypesManagement : ComponentBase
    {
        [Inject]
        private IRoomTypeService RoomTypeService { get; set; } = default!;

        // تم تغيير مستوى الوصول إلى protected ليتمكن ملف razor من رؤيتها
        protected List<RoomType>? roomTypes;
        protected RoomType currentRoomType = new();
        protected bool isEditing = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadRoomTypes();
        }

        private async Task LoadRoomTypes()
        {
            roomTypes = await RoomTypeService.GetAllRoomTypesAsync();
        }

        protected async Task HandleSave()
        {
            if (string.IsNullOrWhiteSpace(currentRoomType.Name))
                return; // منع إضافة اسم فارغ

            if (isEditing)
            {
                await RoomTypeService.UpdateRoomTypeAsync(currentRoomType);
            }
            else
            {
                await RoomTypeService.AddRoomTypeAsync(currentRoomType);
            }

            currentRoomType = new(); // Reset form
            isEditing = false;
            await LoadRoomTypes();
            StateHasChanged(); // إخبار الواجهة أن هناك تحديثًا يجب عرضه
        }

        protected void StartEdit(RoomType roomTypeToEdit)
        {
            // Create a copy for editing
            currentRoomType = new RoomType
            {
                Id = roomTypeToEdit.Id,
                Name = roomTypeToEdit.Name
            };
            isEditing = true;
        }

        protected void CancelEdit()
        {
            currentRoomType = new();
            isEditing = false;
        }

        protected async Task HandleDelete(int id)
        {
            // يمكنك هنا إضافة نافذة تأكيد الحذف لاحقًا
            await RoomTypeService.DeleteRoomTypeAsync(id);
            await LoadRoomTypes();
            StateHasChanged(); // إخبار الواجهة أن هناك تحديثًا يجب عرضه
        }
    }
}