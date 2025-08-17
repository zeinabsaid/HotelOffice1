using HotelOffice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelOffice.Business.Interfaces
{
    public interface IFloorService
    {
        Task<List<Floor>> GetAllFloorsAsync();
        Task AddFloorAsync(Floor floor);
        Task UpdateFloorAsync(Floor floor);
        Task DeleteFloorAsync(int id);
    }
}