using HotelOffice.Data;
using HotelOffice.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelOffice.Business.Services
{
    public class GuestService : IGuestService
    {
        private readonly ApplicationDbContext _db;

        public GuestService(ApplicationDbContext context)
        {
            _db = context;
        }

        // ==> تم تحديث هذه الدالة بالكامل لتشمل منطق الـ Include
        public async Task<IEnumerable<Guest>> GetAllAsync(Expression<Func<Guest, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Guest> query = _db.Guests;

            // 1. تطبيق الفلتر (للبحث)
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // 2. تطبيق الـ Include لجلب البيانات المرتبطة
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            return await query.ToListAsync();
        }

        public async Task<Guest?> GetByIdAsync(int id)
        {
            // ملاحظة: GetByIdAsync لا تحتاج لـ include هنا لأننا غالبًا ما نريد
            // فقط الكائن الأساسي. إذا احتجنا بيانات مرتبطة مع Id محدد،
            // يمكننا استخدام GetAllAsync مع فلتر الـ Id.
            return await _db.Guests.FindAsync(id);
        }

        public async Task CreateAsync(Guest guest)
        {
            await _db.Guests.AddAsync(guest);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guest guest)
        {
            _db.Guests.Update(guest);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var guest = await _db.Guests.FindAsync(id);
            if (guest != null)
            {
                _db.Guests.Remove(guest);
                await _db.SaveChangesAsync();
            }
        }
    }
}