using HotelOffice.Business.Interfaces;
using HotelOffice.State;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace HotelOffice.Components.Pages
{
    public partial class Login
    {
        [Inject]
        private IUserService UserService { get; set; } = null!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        private LoginModel _loginModel = new();
        private bool _isLoggingIn = false;
        private string _errorMessage = string.Empty;

        private async Task HandleLogin()
        {
            _isLoggingIn = true;
            _errorMessage = string.Empty;

            var authenticatedUser = await UserService.AuthenticateAsync(_loginModel.Username, _loginModel.Password);

            if (authenticatedUser != null)
            {
                AppState.Login(authenticatedUser);
                NavigationManager.NavigateTo("/");
            }
            else
            {
                _errorMessage = "Invalid username or password.";
                _isLoggingIn = false;
                StateHasChanged(); // ==> إضافة مهمة لتحديث الواجهة بعد الخطأ
            }
        }

        public class LoginModel
        {
            [Required] public string Username { get; set; } = string.Empty;
            [Required] public string Password { get; set; } = string.Empty;
        }
    }
}