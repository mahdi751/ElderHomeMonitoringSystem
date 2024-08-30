using ElderHomeMonitoringSystem.Data;
using ElderHomeMonitoringSystem.Interfaces;
using ElderHomeMonitoringSystem.Models;

namespace ElderHomeMonitoringSystem.Repository
{
    public class SleepPositionRepository : ISleepPositionRepository
    {
        private readonly DataContext _context;

        public SleepPositionRepository(DataContext context)
        {
            _context = context;
        }

        public void AddSleepPosition(SleepPosition sleepPosition)
        {
            _context.SleepPositions.Add(sleepPosition);
            _context.SaveChanges();
        }

        public string GetMostCommonSleepPosition(int userId)
        {
            var mostCommonPosition = _context.SleepPositions
                .Where(sp => sp.UserId == userId)
                .GroupBy(sp => sp.Position)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            return mostCommonPosition;
        }

        public string GetMostCommonSleepPositionWithinInterval(int userId, DateTime startTime, DateTime endTime)
        {
            var mostCommonPosition = _context.SleepPositions
                .Where(sp => sp.UserId == userId && sp.AtTime >= startTime && sp.AtTime <= endTime)
                .GroupBy(sp => sp.Position)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            return mostCommonPosition;
        }
    }
}
