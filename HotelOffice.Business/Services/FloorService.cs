using HotelOffice.Business.Interfaces;
using HotelOffice.Data;
using HotelOffice.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelOffice.Business.Services
{
    public class FloorService : IFloorService
    {
        private readonly ApplicationDbContext _context;

        public FloorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Floor>> GetAllFloorsAsync()
        {
            return await _context.Floors.AsNoTracking().ToListAsync();
        }

        public async Task AddFloorAsync(Floor floor)
        {
            _context.Floors.Add(floor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFloorAsync(Floor floor)
        {
            _context.Entry(floor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFloorAsync(int id)
        {
            var floor = await _context.Floors.FindAsync(id);
            if (floor != null)
            {
                _context.Floors.Remove(floor);
                await _context.SaveChangesAsync();
            }
        }
    }
}