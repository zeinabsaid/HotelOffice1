using HotelOffice.Business.Services;
using HotelOffice.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace HotelOffice.Components.Pages
{
    public partial class UpsertGuest : ComponentBase
    {
        [Inject] private IGuestService GuestService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [Parameter] public int GuestId { get; set; }

        protected Guest _guest = new Guest();
        protected bool IsLoading { get; set; } = true;
        protected bool IsSaving { get; set; } = false;
        protected string? _imageDataUrl;

        private IBrowserFile? _selectedFile;

        protected override async Task OnParametersSetAsync()
        {
            IsLoading = true;
            try
            {
                if (GuestId != 0)
                {
                    var existingGuest = await GuestService.GetByIdAsync(GuestId);
                    if (existingGuest != null) _guest = existingGuest;
                }
                else
                {
                    _guest = new Guest();
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected async Task HandleUpsertGuest()
        {
            IsSaving = true;

            if (_selectedFile != null)
            {
                // TODO: حفظ الصورة في مكان مناسب
            }

            if (_guest.Id == 0)
                await GuestService.CreateAsync(_guest);
            else
                await GuestService.UpdateAsync(_guest);

            IsSaving = false;
            GoToGuestList();
        }

        protected async Task HandleFileSelection(InputFileChangeEventArgs e)
        {
            try
            {
                _selectedFile = e.File;

                if (_selectedFile != null)
                {
                    if (_selectedFile.Size > 5 * 1024 * 1024)
                    {
                        // عرض رسالة خطأ للمستخدم بدلاً من الكراش
                        Console.WriteLine("⚠ حجم الملف أكبر من 5 ميجابايت");
                        return;
                    }

                    var buffer = new byte[_selectedFile.Size];
                    await _selectedFile.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024)
                                       .ReadAsync(buffer);

                    _imageDataUrl = $"data:{_selectedFile.ContentType};base64,{Convert.ToBase64String(buffer)}";
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"خطأ أثناء تحميل الصورة: {ex.Message}");
                // ممكن تضيف Snackbar أو Alert هنا للمستخدم
            }
        }

        protected void GoToGuestList()
        {
            NavigationManager.NavigateTo("/guests");
        }
    }
}
