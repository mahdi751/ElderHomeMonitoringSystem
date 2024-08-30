using ElderHomeMonitoringSystem.Models;

namespace ElderHomeMonitoringSystem.Interfaces
{
    public interface ISleepPositionRepository
    {
        void AddSleepPosition(SleepPosition sleepPosition);
        string GetMostCommonSleepPosition(int userId);
        string GetMostCommonSleepPositionWithinInterval(int userId, DateTime startTime, DateTime endTime);
    }
}
