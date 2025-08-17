// In HotelOffice.Business/IRoomTypeService.cs

using HotelOffice.Models;

namespace HotelOffice.Business.Interfaces
{
    public interface IRoomTypeService
    {
        Task<List<RoomType>> GetAllRoomTypesAsync();
        Task<RoomType?> GetRoomTypeByIdAsync(int id);
        Task<bool> AddRoomTypeAsync(RoomType roomType);
        Task<bool> UpdateRoomTypeAsync(RoomType roomType);
        Task<bool> DeleteRoomTypeAsync(int id);
    }
}