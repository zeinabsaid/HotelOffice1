using HotelOffice.Data;
using HotelOffice.Models;
using Microsoft.EntityFrameworkCore;
// لا حاجة لـ using BCrypt.Net; هنا لأننا سنستخدم الاسم الكامل

namespace HotelOffice.Business.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;

        public UserService(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

            if (user == null)
            {
                return null;
            }

            // ==> التعديل: استخدام الاسم الكامل للمكتبة BCrypt.Net.BCrypt
            if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }

        public async Task CreateAsync(User user)
        {
            // ==> التعديل: استخدام الاسم الكامل للمكتبة BCrypt.Net.BCrypt
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
        }
    }
}