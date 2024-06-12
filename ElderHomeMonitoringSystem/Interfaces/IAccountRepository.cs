using ElderHomeMonitoringSystem.Models;

namespace ElderHomeMonitoringSystem.Interfaces
{
    public interface IAccountRepository
    {
        public Task<User> GetUserByUsername(string username);
        public Task<int> GetUserIDByUsername(string username);
        public Task<string> GetUsernamaByID(int id);
        public Task<bool> AddUser(User user);
        public Task<bool> UpdateUser(User user);
        public Task<bool> UserExits(string username);
        public Task<bool> UserExits(User user);
        public Task<bool> EmailExists(string email);
        public Task<bool> EmailExists(User user);
        public Task<User> GetUserByID(int userid);
        public Task<bool> RemoveUser(User user);
        public Task<bool> Save();
    }
}
