using HotelOffice.ViewModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace HotelOffice.Components.Pages
{
    public partial class ReceptionistDashboardBase : ComponentBase, IDisposable
    {
        [Inject]
        protected ReceptionistDashboardViewModel ViewModel { get; set; } = default!; // <== تم التعديل هنا

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!; // <== تم التعديل 

        protected override async Task OnInitializedAsync()
        {
            ViewModel.PropertyChanged += (s, e) => InvokeAsync(StateHasChanged);
            if (ViewModel.LoadRoomStatusesCommand.CanExecute(null))
            {
                await ViewModel.LoadRoomStatusesCommand.ExecuteAsync(null);
            }
        }

        protected void NavigateToBooking(int roomId)
        {
            NavigationManager.NavigateTo($"/new-booking/{roomId}");
        }

        protected void NavigateToBookingDetails(int bookingId)
        {
            NavigationManager.NavigateTo($"/booking-details/{bookingId}");
        }

        public void Dispose()
        {
            ViewModel.PropertyChanged -= (s, e) => InvokeAsync(StateHasChanged);
        }
    }
}