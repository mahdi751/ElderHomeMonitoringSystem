using ElderHomeMonitoringSystem.Data;
using ElderHomeMonitoringSystem.Interfaces;
using ElderHomeMonitoringSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ElderHomeMonitoringSystem.Repository
{
    public class MovementRepository : IMovementRepository
    {
        private readonly DataContext _context;
        public MovementRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovementLog>> GetAllAsync()
        {
            return await _context.MovementLogs.ToListAsync();
        }
        public async Task<IEnumerable<MovementLog>> GetAllByUserIdAsync(int id)
        {
            return await _context.MovementLogs.Where(m => m.UserID == id).ToListAsync();
        }

        public async Task<MovementLog> GetByIdAsync(int id)
        {
            return await _context.MovementLogs.FindAsync(id);
        }

        public async Task AddAsync(MovementLog movementLog)
        {
            await _context.MovementLogs.AddAsync(movementLog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MovementLog movementLog)
        {
            _context.MovementLogs.Update(movementLog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var movementLog = await _context.MovementLogs.FindAsync(id);
            if (movementLog != null)
            {
                _context.MovementLogs.Remove(movementLog);
                await _context.SaveChangesAsync();
            }
        }
    }
}
