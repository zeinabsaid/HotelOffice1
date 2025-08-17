using HotelOffice.Data;
using HotelOffice.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using HotelOffice.Business.Interfaces;

namespace HotelOffice.Business.Services
{
    public class GuestService : IGuestService
    {
        private readonly ApplicationDbContext _db;

        public GuestService(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Guest>> GetAllAsync(Expression<Func<Guest, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Guest> query = _db.Guests;

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

        public async Task<Guest?> GetByIdAsync(int id)
        {
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

        public async Task<Guest> FindOrCreateGuestAsync(string fullName, string phoneNumber, string nationalId)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
            {
                // إذا لم يكن الرقم القومي موجودًا، أنشئ نزيلًا جديدًا دائمًا
                return await CreateNewGuest(fullName, phoneNumber, nationalId);
            }

            var existingGuest = await _db.Guests.FirstOrDefaultAsync(g => g.NationalId != null && g.NationalId.ToLower() == nationalId.ToLower());

            if (existingGuest != null)
            {
                return existingGuest;
            }

            return await CreateNewGuest(fullName, phoneNumber, nationalId);
        }

        private async Task<Guest> CreateNewGuest(string fullName, string phoneNumber, string nationalId)
        {
            var newGuest = new Guest
            {
                FullName = fullName,
                PhoneNumber = phoneNumber,
                NationalId = nationalId
            };
            await CreateAsync(newGuest);
            return newGuest;
        }
    }
}