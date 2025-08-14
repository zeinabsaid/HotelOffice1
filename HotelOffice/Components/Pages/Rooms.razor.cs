using HotelOffice.Business.Services;
using HotelOffice.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelOffice.Components.Pages
{
    public partial class Rooms
    {
        [Inject]
        private IRoomService RoomService { get; set; } = null!;

        private IEnumerable<Room> _roomList = new List<Room>();
        public string SearchTerm { get; set; } = string.Empty;

        // ==> متغير جديد لتخزين الغرفة المختارة لعرض التفاصيل
        private Room? _selectedRoom;

        protected override async Task OnInitializedAsync()
        {
            await LoadRooms();
        }

        private async Task LoadRooms()
        {
            _roomList = await RoomService.GetAllAsync(
                filter: string.IsNullOrWhiteSpace(SearchTerm)
                    ? null
                    : r => r.RoomNumber.Contains(SearchTerm) || (r.RoomType != null && r.RoomType.Contains(SearchTerm))
            );
        }

        private async Task OnSearchInput(ChangeEventArgs e)
        {
            SearchTerm = e.Value?.ToString() ?? string.Empty;
            await LoadRooms();
        }

        private async Task DeleteRoom(int roomId)
        {
            await RoomService.DeleteAsync(roomId);
            await LoadRooms(); // أعد تحميل القائمة
        }

        // ==> دوال جديدة للتحكم في المودال
        private void ShowDetails(Room room)
        {
            _selectedRoom = room;
        }

        private void CloseDetailsModal()
        {
            _selectedRoom = null;
        }
    }
}