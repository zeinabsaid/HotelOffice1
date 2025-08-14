using HotelOffice.Models;
using System.Linq.Expressions;

namespace HotelOffice.Business.Services
{
    public interface IGuestService
    {
        // ==> تم التحديث هنا لإضافة includeProperties
        Task<IEnumerable<Guest>> GetAllAsync(
            Expression<Func<Guest, bool>>? filter = null,
            string? includeProperties = null
        );

        Task<Guest?> GetByIdAsync(int id);
        Task CreateAsync(Guest guest);
        Task UpdateAsync(Guest guest);
        Task DeleteAsync(int id);
    }
}