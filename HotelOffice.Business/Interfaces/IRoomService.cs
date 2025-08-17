using HotelOffice.Models;

using System.Linq.Expressions; // ==> هذا السطر ضروري

namespace HotelOffice.Business.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllAsync(
            Expression<Func<Room, bool>>? filter = null,
            string? includeProperties = null
        );
        Task<Room?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Room>> GetAllWithDetailsAsync();
        Task<Room?> GetByIdAsync(int id);
        Task CreateAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(int id);
    }
}
