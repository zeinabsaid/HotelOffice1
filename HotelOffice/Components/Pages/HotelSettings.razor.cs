using HotelOffice.Business.Interfaces;
using HotelOffice.Models; // تأكد من وجود هذا السطر
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace HotelOffice.Components.Pages
{
    public partial class HotelSettings : ComponentBase
    {
        [Inject]
        private IHotelSettingsService SettingsService { get; set; } = default!;

        protected Setting settings = new();

        protected string successMessage = string.Empty;
        protected string errorMessage = string.Empty;
        protected bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            settings = await SettingsService.GetSettingsAsync();
            isLoading = false;
        }

        protected async Task HandleSave()
        {
            try
            {
                await SettingsService.SaveSettingsAsync(settings);
                successMessage = "تم حفظ الإعدادات بنجاح!";
                errorMessage = string.Empty;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"حدث خطأ أثناء الحفظ: {ex.Message}";
                successMessage = string.Empty;
            }
        }
    }
}