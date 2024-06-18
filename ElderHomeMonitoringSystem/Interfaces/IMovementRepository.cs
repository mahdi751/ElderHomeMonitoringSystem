using ElderHomeMonitoringSystem.Models;

namespace ElderHomeMonitoringSystem.Interfaces
{
    public interface IMovementRepository 
    {
        Task<IEnumerable<MovementLog>> GetAllAsync();
        Task<MovementLog> GetByIdAsync(int id);
        Task AddAsync(MovementLog movementLog);
        Task UpdateAsync(MovementLog movementLog);
        Task DeleteAsync(int id);
        Task<IEnumerable<MovementLog>> GetAllByUserIdAsync(int id);
    }
}
