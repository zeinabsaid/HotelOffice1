using HotelOffice.Business.Interfaces;
using HotelOffice.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Maui.Storage; // ==> السطر المهم لمنصة MAUI
using System;
using System.IO;
using System.Threading.Tasks;

namespace HotelOffice.Components.Pages
{
    public partial class UpsertGuest
    {
        [Inject] private IGuestService GuestService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [Parameter] public int GuestId { get; set; }

        // ==============================================================
        //      هذه هي المتغيرات التي كانت مفقودة وسببت الأخطاء
        // ==============================================================
        protected Guest _guest = new();
        protected bool IsLoading { get; set; } = true;
        protected bool IsSaving { get; set; } = false;
        protected string? _imageDataUrl;
        private IBrowserFile? _selectedFile;
        // ==============================================================

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
                string? imagePath = await SaveImageAndGetPathAsync(_selectedFile);
                _guest.IdCardImageUrl = imagePath;
            }

            if (_guest.Id == 0)
                await GuestService.CreateAsync(_guest);
            else
                await GuestService.UpdateAsync(_guest);

            IsSaving = false;
            GoToGuestList();
        }

        private async Task<string?> SaveImageAndGetPathAsync(IBrowserFile file)
        {
            try
            {
                // استخدام المسار المعتمد في MAUI لضمان الوصول للملفات
                var imagesFolder = Path.Combine(FileSystem.AppDataDirectory, "GuestImages");

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";
                var fullPath = Path.Combine(imagesFolder, fileName);

                await using FileStream fs = new(fullPath, FileMode.Create);
                await file.OpenReadStream(5 * 1024 * 1024).CopyToAsync(fs);

                return fullPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image: {ex.Message}");
                return null;
            }
        }

        protected async Task HandleFileSelection(InputFileChangeEventArgs e)
        {
            _selectedFile = e.File;
            if (_selectedFile == null) return;

            try
            {
                var buffer = new byte[_selectedFile.Size];
                await _selectedFile.OpenReadStream(5 * 1024 * 1024).ReadAsync(buffer);
                _imageDataUrl = $"data:{_selectedFile.ContentType};base64,{Convert.ToBase64String(buffer)}";
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }

        protected void GoToGuestList()
        {
            NavigationManager.NavigateTo("/guests");
        }
    }
}