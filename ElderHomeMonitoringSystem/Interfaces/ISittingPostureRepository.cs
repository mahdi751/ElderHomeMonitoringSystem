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


        Task<IEnumerable<SittingPosture>> GetPosturesByDateAsync(DateTime date,int UserId);
        Task<IEnumerable<SittingPosture>> GetPosturesByWeekAsync(DateTime startDate, int UserId);
        Task<IEnumerable<SittingPosture>> GetPosturesByMonthAsync(int month, int year, int UserId);
        Task<double> GetDailyGoodPosturePercentage(DateTime date, int UserId);
        Task<double> GetWeeklyGoodPosturePercentage(DateTime startDate, int UserId);
        Task<double> GetMonthlyGoodPosturePercentage(int month, int year, int UserId);
    }
}
