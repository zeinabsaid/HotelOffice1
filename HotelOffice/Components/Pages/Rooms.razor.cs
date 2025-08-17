using HotelOffice.Business.Interfaces;
using HotelOffice.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelOffice.Components.Pages
{
    public partial class RoomsBase : ComponentBase
    {
        [Inject]
        protected IRoomService RoomService { get; set; } = default!;

        protected List<Room> RoomList { get; set; } = new List<Room>();
        protected Room? SelectedRoom { get; set; }
        protected bool IsLoading { get; set; } = true;
        protected string SearchTerm { get; set; } = string.Empty;
        protected int? RoomIdToDelete { get; set; }
        protected string RoomNumberToDelete { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await LoadRooms();
        }

        protected async Task LoadRooms()
        {
            IsLoading = true;
            RoomList = (await RoomService.GetAllWithDetailsAsync()).ToList();
            IsLoading = false;
        }

        protected async Task SearchRooms()
        {
            IsLoading = true;
            await InvokeAsync(StateHasChanged);
            var allRooms = await RoomService.GetAllWithDetailsAsync();
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                RoomList = allRooms.ToList();
            }
            else
            {
                var searchTermLower = SearchTerm.ToLower();
                RoomList = allRooms.Where(r =>
                    r.RoomNumber.ToLower().Contains(searchTermLower) ||
                    (r.RoomType != null && r.RoomType.Name.ToLower().Contains(searchTermLower))
                ).ToList();
            }
            IsLoading = false;
            await InvokeAsync(StateHasChanged);
        }

        protected void ShowDetails(Room room)
        {
            SelectedRoom = room;
        }

        protected void CloseDetailsModal()
        {
            SelectedRoom = null;
        }

        protected void SetRoomForDeletion(int id, string roomNumber)
        {
            RoomIdToDelete = id;
            RoomNumberToDelete = roomNumber;
        }

        protected async Task ConfirmDelete()
        {
            if (RoomIdToDelete.HasValue)
            {
                await RoomService.DeleteAsync(RoomIdToDelete.Value);
                RoomList.RemoveAll(r => r.Id == RoomIdToDelete.Value);
                RoomIdToDelete = null;
                StateHasChanged();
            }
        }

        protected void CancelDelete()
        {
            RoomIdToDelete = null;
        }

        // --- ✅ الدالة الجديدة والمحدثة ---
        // تقوم بتحويل مصفوفة البايت إلى صورة يمكن عرضها في المتصفح
        protected string ConvertByteArrayToDataUrl(byte[]? imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                // تأكد من وجود هذه الصورة الافتراضية في مجلد wwwroot/images
                return "images/placeholder.png";
            }
            var base64String = Convert.ToBase64String(imageData);
            return $"data:image/jpeg;base64,{base64String}";
        }
    }
}