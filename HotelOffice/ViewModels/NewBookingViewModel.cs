using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelOffice.Business.Interfaces;
using HotelOffice.Models;
using HotelOffice.State;
using System.Threading.Tasks;

namespace HotelOffice.ViewModels
{
    public partial class NewBookingViewModel : ObservableObject
    {
        private readonly IBookingService _bookingService;
        private readonly IGuestService _guestService;
        private readonly IRoomService _roomService;

        [ObservableProperty]
        private DateTime _checkInDate = DateTime.Today;

        [ObservableProperty]
        private DateTime _checkOutDate = DateTime.Today.AddDays(1);

        [ObservableProperty]
        private string _guestFullName = string.Empty; // تهيئة بقيمة افتراضية

        [ObservableProperty]
        private string _guestPhoneNumber = string.Empty; // تهيئة بقيمة افتراضية

        [ObservableProperty]
        private string _guestNationalId = string.Empty; // تهيئة بقيمة افتراضية

        [ObservableProperty]
        private decimal _amountPaid;

        [ObservableProperty]
        private Room? _room; // جعلها قابلة للـ null

        [ObservableProperty]
        private string _errorMessage = string.Empty; // تهيئة بقيمة افتراضية

        [ObservableProperty]
        private bool _isBusy;

        public NewBookingViewModel(IBookingService bookingService, IGuestService guestService, IRoomService roomService)
        {
            _bookingService = bookingService;
            _guestService = guestService;
            _roomService = roomService;
        }

        public async Task LoadRoomDetailsAsync(int roomId)
        {
            // ===> تم التغيير هنا لاستخدام الدالة التي تجلب التفاصيل <===
            Room = await _roomService.GetByIdWithDetailsAsync(roomId);
        }

        [RelayCommand]
        private async Task CreateBookingAsync()
        {
            if (string.IsNullOrWhiteSpace(GuestFullName))
            {
                ErrorMessage = "يجب إدخال اسم النزيل.";
                return;
            }
            if (CheckOutDate <= CheckInDate)
            {
                ErrorMessage = "تاريخ المغادرة يجب أن يكون بعد تاريخ الوصول.";
                return;
            }
            if (Room == null || !AppState.IsLoggedIn)
            {
                ErrorMessage = "خطأ في النظام. لا يمكن تحديد الغرفة أو المستخدم.";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var guest = await _guestService.FindOrCreateGuestAsync(GuestFullName, GuestPhoneNumber, GuestNationalId);

                var newBooking = new Booking
                {
                    RoomId = Room.Id,
                    GuestId = guest.Id,
                    UserId = AppState.CurrentUser.Id,
                    CheckInDate = CheckInDate,
                    CheckOutDate = CheckOutDate,
                    AmountPaid = AmountPaid,
                    BookingStatus = "Confirmed"
                };

                await _bookingService.CreateAsync(newBooking);
                // يمكنك هنا إضافة أمر للانتقال لصفحة أخرى بعد النجاح
            }
            catch (InvalidOperationException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch (Exception)
            {
                ErrorMessage = "حدث خطأ غير متوقع أثناء محاولة إنشاء الحجز.";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}