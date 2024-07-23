using ElderHomeMonitoringSystem.DTOs;
using ElderHomeMonitoringSystem.Models;

namespace ElderHomeMonitoringSystem.Interfaces
{
    public interface ISittingPostureRepository
    {
        Task<IEnumerable<SittingPosture>> GetAll(DateTime startDate, DateTime endDate);
        Task<List<SittingPosture>> GetAllPosturesAsync();
        Task<SittingPosture> GetPostureByIdAsync(int id);
        Task AddPostureAsync(SittingPosture posture);
        Task<bool> UpdatePostureAsync(SittingPosture posture);
        Task<bool> DeletePostureAsync(int id);
        Task<string> ProvideFeedbackAsync(SittingPosture latestData);
        Task<DailySummary> GetDailySummaryAsync(int userId);
        Task<List<TrendData>> GetTrendsAsync(int userId);
        Task<PostureStatistics> GetPostureStatisticsAsync(int userId);
        Task<PostureStatistics> GetFilteredPostureStatisticsAsync(int userId, string period);
        Task<IEnumerable<PostureStatistics>> GetStatisticsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<PostureStatistics>> GetAllStatisticsAsync();

/*        Task UpdateGoalsAsync(int userId, PostureGoals goals);
        Task<PostureGoals> GetGoalsAsync(int userId);*/
    }
}
