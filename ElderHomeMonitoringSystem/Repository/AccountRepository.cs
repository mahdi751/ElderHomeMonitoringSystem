using ElderHomeMonitoringSystem.Data;
using ElderHomeMonitoringSystem.Interfaces;
using ElderHomeMonitoringSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

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
            try
            {
                var users = await _context.Users.ToListAsync();
                return users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.Ordinal));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserByUsername: {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetUserIDByUsername(string username)
        {
            try
            {
                return await _context.Users.Where(u => u.Username == username)
                                           .Select(u => u.UserID)
                                           .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserIDByUsername: {ex.Message}");
                throw;
            }
        }
        public async Task<int> GetUserIdByMac(string Macadress)
        {
            try
            {
                return await _context.Users.Where(u => u.MacAddress == Macadress)
                                           .Select(u => u.UserID)
                                           .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserIdByMac: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UserExits(User user)
        {
            try
            {
                return await _context.Users.AnyAsync(x => x.Username.ToLower() == user.Username.ToLower() && x.UserID != user.UserID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UserExits(User): {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UserExits(string username)
        {
            try
            {
                return await _context.Users.AnyAsync(x => x.Username == username.ToLower());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UserExits(string): {ex.Message}");
                throw;
            }
        }

        public async Task<bool> EmailExists(string email)
        {
            try
            {
                return await _context.Users.AnyAsync(x => x.Email == email.ToLower());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in EmailExists(string): {ex.Message}");
                throw;
            }
        }

        public async Task<byte[]> GetUserImage(int userId)
        {
            return _context.Users.Where(u => u.UserID == userId).Select(u => u.ProfileImage).FirstOrDefault();
        }

        public async Task<bool> EmailExists(User user)
        {
            try
            {
                return await _context.Users.AnyAsync(x => x.Email == user.Email.ToLower() && x.UserID != user.UserID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in EmailExists(User): {ex.Message}");
                throw;
            }
        }

        public async Task<bool> AddUser(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                return await Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddUser: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                _context.Users.Update(user);
                return await Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateUser: {ex.Message}");
                throw;
            }
        }

        public async Task<User> GetUserByID(int userid)
        {
            try
            {
                return await _context.Users.Where(u => u.UserID == userid).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserByID: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetUsernamaByID(int id)
        {
            try
            {
                return await _context.Users.Where(u => u.UserID == id)
                                    .Select(u => u.Username).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUsernamaByID: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> RemoveUser(User user)
        {
            try
            {
                _context.Users.Remove(user);
                return await Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RemoveUser: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetNotActivated()
        {
            try
            {
                return await _context.Users.Where(u => !u.Activated).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetNotActivated: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> Save()
        {
            try
            {
                var saved = await _context.SaveChangesAsync();
                return saved > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Save: {ex.Message}");
                throw;
            }
        }
    }
}
