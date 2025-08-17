// In HotelOffice.Business/RoomTypeService.cs

using HotelOffice.Business.Interfaces;
using HotelOffice.Data;
using HotelOffice.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelOffice.Business.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly ApplicationDbContext _context;

        public RoomTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoomType>> GetAllRoomTypesAsync()
        {
            return await _context.RoomTypes.AsNoTracking().ToListAsync();
        }

        public async Task<RoomType?> GetRoomTypeByIdAsync(int id)
        {
            return await _context.RoomTypes.FindAsync(id);
        }

        public async Task<bool> AddRoomTypeAsync(RoomType roomType)
        {
            if (roomType == null)
                return false;

            _context.RoomTypes.Add(roomType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRoomTypeAsync(RoomType roomType)
        {
            var existingType = await _context.RoomTypes.FindAsync(roomType.Id);
            if (existingType == null)
                return false;

            existingType.Name = roomType.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRoomTypeAsync(int id)
        {
            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType == null)
                return false;

            _context.RoomTypes.Remove(roomType);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}