using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelOffice.Business.Interfaces;
using HotelOffice.Models;
using System.Threading.Tasks;

namespace HotelOffice.ViewModels
{
    public partial class BookingDetailsViewModel : ObservableObject
    {
        private readonly IBookingService _bookingService;

        // الخاصية الرئيسية التي تحمل كل بيانات الحجز
        [ObservableProperty]
        private Booking _booking;

        // خاصية لربطها بحقل إدخال مبلغ الدفعة الجديدة
        [ObservableProperty]
        private decimal _newPaymentAmount;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private string _successMessage;

        public BookingDetailsViewModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // دالة لتحميل الحجز من قاعدة البيانات
        public async Task LoadBookingAsync(int bookingId)
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            try
            {
                // نستخدم GetByIdAsync التي تجلب معها تفاصيل الغرفة والنزيل
                Booking = await _bookingService.GetByIdAsync(bookingId);
                if (Booking == null)
                {
                    ErrorMessage = "لم يتم العثور على الحجز المطلوب.";
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CheckInAsync()
        {
            if (Booking == null) return;
            await UpdateStatusAsync("CheckedIn", "تم تسجيل وصول النزيل بنجاح.");
        }

        [RelayCommand]
        private async Task CheckOutAsync()
        {
            if (Booking == null) return;
            // التحقق من أن المبلغ المتبقي صفر قبل المغادرة
            if (Booking.RemainingBalance > 0)
            {
                ErrorMessage = $"لا يمكن تسجيل المغادرة. يرجى تحصيل المبلغ المتبقي وقدره: {Booking.RemainingBalance:C}";
                return;
            }
            await UpdateStatusAsync("CheckedOut", "تم تسجيل مغادرة النزيل بنجاح.");
        }

        [RelayCommand]
        private async Task RecordPaymentAsync()
        {
            if (Booking == null || NewPaymentAmount <= 0)
            {
                ErrorMessage = "الرجاء إدخال مبلغ صحيح للدفع.";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            try
            {
                await _bookingService.RecordPaymentAsync(Booking.Id, NewPaymentAmount);
                // بعد تسجيل الدفعة، يجب إعادة تحميل بيانات الحجز لإظهار التحديث
                await LoadBookingAsync(Booking.Id);
                SuccessMessage = "تم تسجيل الدفعة بنجاح.";
                NewPaymentAmount = 0; // تصفير حقل الإدخال
            }
            catch (Exception ex)
            {
                ErrorMessage = $"حدث خطأ أثناء تسجيل الدفعة: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        // دالة مساعدة لتحديث حالة الحجز لتجنب تكرار الكود
        private async Task UpdateStatusAsync(string newStatus, string successMessage)
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            try
            {
                await _bookingService.UpdateBookingStatusAsync(Booking.Id, newStatus);
                await LoadBookingAsync(Booking.Id); // إعادة تحميل البيانات
                SuccessMessage = successMessage;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"حدث خطأ: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}