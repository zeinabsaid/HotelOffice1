// في ملف RoomService.cs

using HotelOffice.Data;
using HotelOffice.Models;
using Microsoft.EntityFrameworkCore; // ==> هذا السطر هو الذي يحل خطأ ToListAsync
using System.Linq.Expressions;

namespace HotelOffice.Business.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _db;

        public RoomService(ApplicationDbContext context)
        {
            _db = context;
        }

        // هذا هو التنفيذ الكامل والقوي للدالة
        public async Task<IEnumerable<Room>> GetAllAsync(Expression<Func<Room, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Room> query = _db.Rooms;

            // تطبيق الفلتر (للبحث)
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // تطبيق الـ Include لجلب البيانات المرتبطة
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            return await query.ToListAsync();
        }

        // باقي الدوال كما هي
        public async Task<Room?> GetByIdAsync(int id) => await _db.Rooms.FindAsync(id);

        public async Task CreateAsync(Room room)
        {
            await _db.Rooms.AddAsync(room);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room room)
        {
            _db.Rooms.Update(room);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var room = await _db.Rooms.FindAsync(id);
            if (room != null)
            {
                _db.Rooms.Remove(room);
                await _db.SaveChangesAsync();
            }
        }
    }
}