using HotelOffice.Business.Interfaces;
using HotelOffice.Data;
using HotelOffice.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic; // تمت إضافة هذا السطر
using System.Linq; // تمت إضافة هذا السطر
using System.Linq.Expressions;
using System.Threading.Tasks; // تمت إضافة هذا السطر

namespace HotelOffice.Business.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _db;

        public RoomService(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Room>> GetAllAsync(Expression<Func<Room, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Room> query = _db.Rooms;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetAllWithDetailsAsync()
        {
            // تم إضافة Include للمرافق هنا أيضًا لضمان عرضها في التفاصيل
            return await _db.Rooms
                                 .Include(r => r.RoomType)
                                 .Include(r => r.Floor)
                                 .Include(r => r.Amenities) // <== أضف هذا السطر
                                 .AsNoTracking() // جيد للأداء في عمليات القراءة
                                 .ToListAsync();
        }

        public async Task<Room?> GetByIdWithDetailsAsync(int id)
        {
            return await _db.Rooms
                                 .Include(r => r.RoomType)
                                 .Include(r => r.Floor)
                                 .Include(r => r.Amenities) // <== أضف هذا السطر
                                 .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Room?> GetByIdAsync(int id) => await _db.Rooms.FindAsync(id);

        // --- ✅  هذا هو التعديل الأهم لحل مشكلة الحفظ ---
        public async Task CreateAsync(Room room)
        {
            // 1. فصل المرافق عن الغرفة مؤقتًا
            var amenitiesToAdd = room.Amenities.ToList();
            room.Amenities.Clear();

            // 2. إضافة الغرفة الجديدة الفارغة إلى Context
            await _db.Rooms.AddAsync(room);

            // 3. الآن، قم بإرفاق (Attach) المرافق الموجودة بالفعل
            // هذا يخبر EF أنها ليست جديدة
            foreach (var amenity in amenitiesToAdd)
            {
                _db.Amenities.Attach(amenity);
            }

            // 4. أخيرًا، أعد ربط المرافق بالغرفة الجديدة
            room.Amenities = amenitiesToAdd;

            // 5. احفظ كل التغييرات
            await _db.SaveChangesAsync();
        }

        // --- ✅  تم تعديل دالة التحديث أيضًا لتتعامل مع المرافق ---
        public async Task UpdateAsync(Room room)
        {
            var roomFromDb = await _db.Rooms
                                     .Include(r => r.Amenities)
                                     .FirstOrDefaultAsync(r => r.Id == room.Id);

            if (roomFromDb != null)
            {
                // تحديث الخصائص البسيطة
                _db.Entry(roomFromDb).CurrentValues.SetValues(room);

                // فصل المرافق الجديدة
                var newAmenityIds = room.Amenities.Select(a => a.Id).ToHashSet();
                room.Amenities.Clear(); // مسح القائمة المؤقتة

                // تحديث علاقة المرافق
                roomFromDb.Amenities.Clear();
                var amenitiesFromDb = await _db.Amenities.Where(a => newAmenityIds.Contains(a.Id)).ToListAsync();
                roomFromDb.Amenities = amenitiesFromDb;

                await _db.SaveChangesAsync();
            }
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