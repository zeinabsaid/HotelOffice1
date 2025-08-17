using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelOffice.Business.Interfaces;
using HotelOffice.Models;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using System.Linq; // تم إضافة هذا السطر
using System.Threading.Tasks;

namespace HotelOffice.ViewModels
{
    public partial class ReceptionistDashboardViewModel(IBookingService bookingService, IRoomService roomService) : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<RoomStatusViewModel> _roomsWithStatus = new();

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private decimal _dailyRevenue;

        [ObservableProperty]
        private decimal _monthlyRevenue;

        [ObservableProperty]
        private decimal _customRangeRevenue;

        [ObservableProperty]
        private DateTime _customStartDate = DateTime.Today;

        [ObservableProperty]
        private DateTime _customEndDate = DateTime.Today;

        [RelayCommand]
        async Task LoadRoomStatusesAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                RoomsWithStatus.Clear();

                // ===> تم التغيير هنا لاستخدام الدالة التي تجلب تفاصيل نوع الغرفة <===
                var allRooms = await roomService.GetAllWithDetailsAsync();

                var today = DateTime.Today;

                var relevantBookings = await bookingService.GetAllAsync(
                    b => b.CheckOutDate.Date >= today && b.BookingStatus != "CheckedOut" && b.BookingStatus != "Cancelled",
                    includeProperties: "Guest,Room"
                );

                foreach (var room in allRooms)
                {
                    var roomBooking = relevantBookings.FirstOrDefault(b => b.RoomId == room.Id);
                    RoomStatusViewModel statusVM;

                    if (roomBooking == null)
                    {
                        statusVM = new RoomStatusViewModel
                        {
                            Room = room,
                            StatusText = "متاحة",
                            StatusColor = Colors.SeaGreen,
                            ActiveBooking = null
                        };
                    }
                    else
                    {
                        statusVM = new RoomStatusViewModel
                        {
                            Room = room,
                            ActiveBooking = roomBooking,
                            StatusText = roomBooking.BookingStatus == "CheckedIn"
                                ? $"مشغولة - نزيل: {roomBooking.Guest?.FullName ?? "غير معروف"}"
                                : $"محجوزة - وصول: {roomBooking.CheckInDate:dd/MM}",
                            StatusColor = roomBooking.BookingStatus == "CheckedIn"
                                ? Colors.IndianRed
                                : Colors.Orange
                        };
                    }
                    RoomsWithStatus.Add(statusVM);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task CalculateDailyRevenueAsync()
        {
            IsBusy = true;
            try
            {
                DailyRevenue = await bookingService.CalculateRevenueAsync(DateTime.Today, DateTime.Today);
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        async Task CalculateMonthlyRevenueAsync()
        {
            IsBusy = true;
            try
            {
                var today = DateTime.Today;
                var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                MonthlyRevenue = await bookingService.CalculateRevenueAsync(firstDayOfMonth, lastDayOfMonth);
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        async Task CalculateCustomRangeRevenueAsync()
        {
            if (CustomEndDate < CustomStartDate)
            {
                if (App.Current?.MainPage != null)
                    await App.Current.MainPage.DisplayAlert("خطأ", "تاريخ النهاية يجب أن يكون بعد تاريخ البداية.", "موافق");
                return;
            }
            IsBusy = true;
            try
            {
                CustomRangeRevenue = await bookingService.CalculateRevenueAsync(CustomStartDate, CustomEndDate);
            }
            finally { IsBusy = false; }
        }
    }
}