using HotelOffice.Business.Interfaces;
using HotelOffice.Data;
using HotelOffice.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelOffice.Business.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly ApplicationDbContext _context;

        public AmenityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Amenity>> GetAllAmenitiesAsync()
        {
            // --- ✅  هذا هو التعديل الوحيد والمهم ---
            return await _context.Amenities.AsNoTracking().ToListAsync();
        }

        // ... (باقي الدوال Add, Update, Delete تبقى كما هي بدون أي تغيير) ...
        public async Task<Amenity> AddAmenityAsync(Amenity amenity)
        {
            amenity.Id = 0;
            var existingAmenity = await _context.Amenities.FirstOrDefaultAsync(a => a.Name.ToLower().Trim() == amenity.Name.ToLower().Trim());
            if (existingAmenity != null)
            {
                throw new InvalidOperationException($"المرفق '{amenity.Name}' موجود بالفعل.");
            }
            _context.Amenities.Add(amenity);
            await _context.SaveChangesAsync();
            return amenity;
        }

        public async Task UpdateAmenityAsync(Amenity amenity)
        {
            var nameExistsInOther = await _context.Amenities.AnyAsync(a => a.Name.ToLower().Trim() == amenity.Name.ToLower().Trim() && a.Id != amenity.Id);
            if (nameExistsInOther)
            {
                throw new InvalidOperationException($"لا يمكن التعديل، الاسم '{amenity.Name}' مستخدم بالفعل لمرفق آخر.");
            }
            _context.Entry(amenity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAmenityAsync(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity != null)
            {
                _context.Amenities.Remove(amenity);
                await _context.SaveChangesAsync();
            }
        }
    }
}