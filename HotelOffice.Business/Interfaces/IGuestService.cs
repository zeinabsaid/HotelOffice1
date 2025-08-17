using HotelOffice.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelOffice.Business.Interfaces
{
    public interface IGuestService
    {
        Task<IEnumerable<Guest>> GetAllAsync(
            Expression<Func<Guest, bool>>? filter = null,
            string? includeProperties = null
        );

        Task<Guest?> GetByIdAsync(int id);
        Task CreateAsync(Guest guest);
        Task UpdateAsync(Guest guest);
        Task DeleteAsync(int id);

        // ==> تم التغيير من name إلى fullName
        Task<Guest> FindOrCreateGuestAsync(string fullName, string phoneNumber, string nationalId);
    }
}