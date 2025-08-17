using HotelOffice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelOffice.Business.Interfaces
{
    public interface IAmenityService
    {
        Task<List<Amenity>> GetAllAmenitiesAsync();

        // --- ✅  تم التعديل ليعيد الكائن الجديد ---
        Task<Amenity> AddAmenityAsync(Amenity amenity);

        Task UpdateAmenityAsync(Amenity amenity);
        Task DeleteAmenityAsync(int id);
    }
}