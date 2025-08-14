using HotelOffice.Business.Services;
using HotelOffice.Models;
using Microsoft.AspNetCore.Components;

namespace HotelOffice.Components.Pages
{
    public partial class Guests
    {
        [Inject]
        private IGuestService GuestService { get; set; } = null!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        private IEnumerable<Guest>? _guestList;
        private string searchTerm = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await LoadGuests();
        }

        private async Task LoadGuests()
        {
            _guestList = await GuestService.GetAllAsync(
                filter: g => string.IsNullOrWhiteSpace(searchTerm) ||
                             g.FullName.Contains(searchTerm) ||
                             (g.PhoneNumber != null && g.PhoneNumber.Contains(searchTerm))
            );
        }

        private async Task SearchGuests()
        {
            await LoadGuests();
            StateHasChanged();
        }

        private async Task HandleDelete(int guestId)
        {
            // في تطبيق حقيقي، يجب إضافة نافذة تأكيد هنا
            // Is it better to just mark as inactive instead of deleting?
            // For now, we will delete.
            await GuestService.DeleteAsync(guestId);
            await LoadGuests(); // أعد تحميل القائمة بعد الحذف
        }
    }
}