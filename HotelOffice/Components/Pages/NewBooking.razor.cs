using HotelOffice.ViewModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace HotelOffice.Components.Pages
{
    // يجب أن يكون اسم الكلاس هنا مطابقًا لما هو موجود في @inherits في ملف .razor
    public partial class NewBookingBase : ComponentBase, IDisposable
    {

        [Inject]
        protected NewBookingViewModel ViewModel { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int RoomId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ViewModel.PropertyChanged += (s, e) => InvokeAsync(StateHasChanged);
            await ViewModel.LoadRoomDetailsAsync(RoomId);
        }

        protected async Task HandleValidSubmit()
        {
            await ViewModel.CreateBookingCommand.ExecuteAsync(null);

            if (string.IsNullOrEmpty(ViewModel.ErrorMessage) && !ViewModel.IsBusy)
            {
                if (App.Current?.MainPage != null)
                    await App.Current.MainPage.DisplayAlert("نجاح", "تم إنشاء الحجز بنجاح!", "موافق");

                NavigationManager.NavigateTo("/reception-dashboard");
            }
        }
        protected void Cancel()
        {
            NavigationManager.NavigateTo("/reception-dashboard");
        }
        public void Dispose()
        {
            ViewModel.PropertyChanged -= (s, e) => InvokeAsync(StateHasChanged);
        }
    }
}