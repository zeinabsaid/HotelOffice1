using HotelOffice.Models;
using System.Linq.Expressions;

namespace HotelOffice.Business.Interfaces
{
    public interface IUserService
    {
        // دالة للتحقق من هوية المستخدم عند تسجيل الدخول
        Task<User?> AuthenticateAsync(string username, string password);

        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}