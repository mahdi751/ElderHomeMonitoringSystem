using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElderHomeMonitoringSystem.Models;

namespace ElderHomeMonitoringSystem.Interfaces
{
    public interface ISleepSessionRepository
    {
        Task<List<SleepSession>> GetAllSleepSessionsAsync();
        Task<SleepSession> GetSleepSessionByIdAsync(int id);
        Task AddSleepSessionAsync(SleepSession sleepSession);
        Task<bool> UpdateSleepSessionAsync(SleepSession sleepSession);
        Task<bool> DeleteSleepSessionAsync(int id);
        Task<IEnumerable<SleepSession>> GetSleepSessionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
