using HotelOffice.Business.Services;
using HotelOffice.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // ==> متغير جديد لتخزين النزيل المختار لعرض التفاصيل
        private Guest? _selectedGuest;


        protected override async Task OnInitializedAsync()
        {
            await LoadGuests();
        }

        private async Task LoadGuests()
        {
            var allGuests = await GuestService.GetAllAsync();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _guestList = allGuests;
            }
            else
            {
                var lowerCaseSearchTerm = searchTerm.ToLower();
                _guestList = allGuests.Where(g =>
                    g.FullName.ToLower().Contains(lowerCaseSearchTerm) ||
                    (g.PhoneNumber != null && g.PhoneNumber.Contains(searchTerm))
                ).ToList();
            }
        }

        private async Task SearchGuests()
        {
            await LoadGuests();
            StateHasChanged();
        }

        private async Task HandleDelete(int guestId)
        {
            // الأفضل هو إضافة نافذة تأكيد هنا قبل الحذف
            await GuestService.DeleteAsync(guestId);
            await LoadGuests(); // أعد تحميل القائمة بعد الحذف
        }

        // ==> دوال جديدة للتحكم في عرض التفاصيل
        private void ShowDetails(Guest guest)
        {
            _selectedGuest = guest;
        }

        private void CloseDetailsModal()
        {
            _selectedGuest = null;
        }
    }
}