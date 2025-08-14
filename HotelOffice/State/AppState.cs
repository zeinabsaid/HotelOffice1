using HotelOffice.Models;

namespace HotelOffice.State
{
    public static class AppState
    {
        public static User? CurrentUser { get; private set; }

        public static void Login(User user)
        {
            CurrentUser = user;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        // ==> هذا هو السطر المصحح. 
        // لقد تأكدت من أن الكود سليم.
        public static bool IsLoggedIn => CurrentUser != null;
    }
}