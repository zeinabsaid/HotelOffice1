using HotelOffice.ViewModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace HotelOffice.Components.Pages
{
    public partial class BookingDetailsBase : ComponentBase, IDisposable
    {
        [Inject]
        protected BookingDetailsViewModel ViewModel { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int BookingId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ViewModel.PropertyChanged += (s, e) => InvokeAsync(StateHasChanged);
            await ViewModel.LoadBookingAsync(BookingId);
        }

        protected string GetStatusColor(string status)
        {
            return status switch
            {
                "Confirmed" => "orange",
                "CheckedIn" => "red",
                "CheckedOut" => "green",
                "Cancelled" => "gray",
                _ => "black"
            };
        }

        public void Dispose()
        {
            ViewModel.PropertyChanged -= (s, e) => InvokeAsync(StateHasChanged);
        }
    }
}