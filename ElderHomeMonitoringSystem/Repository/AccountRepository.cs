using ElderHomeMonitoringSystem.Data;
using ElderHomeMonitoringSystem.Interfaces;
using ElderHomeMonitoringSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ElderHomeMonitoringSystem.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;
        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var users = await _context.Users.ToListAsync();

            return users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.Ordinal));
        }


        public async Task<int> GetUserIDByUsername(string username)
        {
            return await _context.Users.Where(u => u.Username == username)
                                       .Select(u => u.UserID)
                                       .FirstOrDefaultAsync();
        }

        public async Task<bool> UserExits(User user)
        {
            return await _context.Users.AnyAsync(x => x.Username.ToLower() == user.Username.ToLower() && x.UserID != user.UserID);
        }

        public async Task<bool> UserExits(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username.ToLower());
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email.ToLower());
        }

        public async Task<bool> EmailExists(User user)
        {
            return await _context.Users.AnyAsync(x => x.Email == user.Email.ToLower() && x.UserID != user.UserID);
        }

        public async Task<bool> AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            return await Save();
        }

        public async Task<bool> UpdateUser(User user)
        {
            _context.Users.Update(user);
            return await Save();
        }

        public async Task<User> GetUserByID(int userid)
        {
            return await _context.Users.Where(u => u.UserID == userid).FirstOrDefaultAsync();
        }

        public async Task<string> GetUsernamaByID(int id)
        {
            return await _context.Users.Where(u => u.UserID == id)
                                .Select(u => u.Username).FirstOrDefaultAsync();
        }
        public async Task<bool> RemoveUser(User user)
        {
            _context.Users.Remove(user);
            return await Save();
        }
        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

    }
}
