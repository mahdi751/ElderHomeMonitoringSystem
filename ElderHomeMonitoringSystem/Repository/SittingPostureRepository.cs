using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ElderHomeMonitoringSystem.Data;
using ElderHomeMonitoringSystem.Models;
using ElderHomeMonitoringSystem.DTOs;
using ElderHomeMonitoringSystem.Interfaces;

namespace ElderHomeMonitoringSystem.Repository
{
    public class SittingPostureRepository : ISittingPostureRepository
    {
        private readonly DataContext _context;

        public SittingPostureRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<List<SittingPosture>> GetAllPosturesAsync()
        {
            return await _context.SittingPostures.ToListAsync();
        }
        public async Task<IEnumerable<SittingPosture>> GetAll(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date must be less than or equal to end date");
            }

            var postures = await _context.SittingPostures
                .Where(p => p.Time >= startDate && p.Time <= endDate)
                .ToListAsync();

            return postures;
        }



        public async Task<SittingPosture> GetPostureByIdAsync(int id)
        {
            return await _context.SittingPostures.FindAsync(id);
        }

        public async Task AddPostureAsync(SittingPosture posture)
        {
            _context.SittingPostures.Add(posture);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePostureAsync(SittingPosture posture)
        {
            if (!_context.SittingPostures.Any(p => p.Id == posture.Id))
            {
                return false;
            }

            _context.Entry(posture).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.SittingPostures.Any(p => p.Id == posture.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeletePostureAsync(int id)
        {
            var posture = await _context.SittingPostures.FindAsync(id);
            if (posture == null)
            {
                return false;
            }

            _context.SittingPostures.Remove(posture);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> ProvideFeedbackAsync(SittingPosture latestData)
        {
            // Simulating an async task, consider removing if actual database operations are performed here.
            await Task.CompletedTask;
            if (latestData.GoodPosture)
            {
                return "Your posture is good. Keep it up!";
            }
            else
            {
                return "Your posture needs improvement. Try to sit upright.";
            }
        }

        public async Task<PostureStatistics> GetPostureStatisticsAsync(int userId)
        {
            var postures = await _context.SittingPostures.Where(p => p.UserID == userId).ToListAsync();
            return CalculatePostureStatistics(postures);
        }

        public async Task<PostureStatistics> GetFilteredPostureStatisticsAsync(int userId, string period)
        {
            var postures = await _context.SittingPostures.Where(p => p.UserID == userId).ToListAsync();
            return FilterStatisticsByPeriod(postures, period);
        }

        private PostureStatistics CalculatePostureStatistics(List<SittingPosture> postures)
        {
            var totalDuration = postures.Sum(p => p.PostureDuration);
            return new PostureStatistics
            {
                TotalSittingTime = totalDuration,
                GoodPostureTime = postures.Where(p => p.GoodPosture).Sum(p => p.PostureDuration),
                BadPostureTime = postures.Where(p => !p.GoodPosture).Sum(p => p.PostureDuration)
            };
        }
        private PostureStatistics FilterStatisticsByPeriod(List<SittingPosture> postures, string period)
        {
            var filteredPostures = postures.GroupBy(p =>
                period switch
                {
                    "hour" => p.Time.ToString("yyyy-MM-dd HH:00"),
                    "day" => p.Time.ToString("yyyy-MM-dd"),
                    "month" => p.Time.ToString("yyyy-MM"),
                    "year" => p.Time.ToString("yyyy"),
                    _ => p.Time.ToString("yyyy-MM-dd")
                })
                .SelectMany(group => group.ToList())
                .ToList();

            return CalculatePostureStatistics(filteredPostures);
        }


        public async Task<List<TrendData>> GetTrendsAsync(int userId)
        {
            var postures = await _context.SittingPostures.Where(p => p.UserID == userId).ToListAsync();
            return postures.GroupBy(p => p.Time.Date)
                .Select(group => new TrendData
                {
                    Date = group.Key,
                    GoodPosturePercentage = group.Where(p => p.GoodPosture).Sum(p => p.PostureDuration) / group.Sum(p => p.PostureDuration) * 100
                }).ToList();
        }

        public async Task<DailySummary> GetDailySummaryAsync(int userId)
        {
            var today = DateTime.Today;
            var todaysPostures = await _context.SittingPostures
                .Where(p => p.UserID == userId && p.Time.Date == today)
                .ToListAsync();

            return new DailySummary
            {
                UserId = userId,
                Date = today,
                GoodPostureDuration = todaysPostures.Where(p => p.GoodPosture).Sum(p => p.PostureDuration),
                BadPostureDuration = todaysPostures.Where(p => !p.GoodPosture).Sum(p => p.PostureDuration)
            };
        }

        /* public async Task UpdateGoalsAsync(int userId, PostureGoals goals)
         {
             var existingGoals = await _context.PostureGoals.FirstOrDefaultAsync(g => g.UserId == userId);
             if (existingGoals != null)
             {
                 _context.Entry(existingGoals).CurrentValues.SetValues(goals);
             }
             else
             {
                 _context.PostureGoals.Add(goals);
             }
             await _context.SaveChangesAsync();
         }

         public async Task<PostureGoals> GetGoalsAsync(int userId)
         {
             return await _context.PostureGoals.FirstOrDefaultAsync(g => g.UserId == userId);
         }*/
        public async Task<IEnumerable<PostureStatistics>> GetStatisticsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date must be less than or equal to end date");
            }

            var postures = await _context.SittingPostures
                .Where(p => p.Time >= startDate && p.Time <= endDate)
                .ToListAsync();

            var statistics = postures
                .GroupBy(p => p.UserID)
                .Select(group => new PostureStatistics
                {
                    TotalSittingTime = group.Sum(p => p.PostureDuration),
                    GoodPostureTime = group.Where(p => p.GoodPosture).Sum(p => p.PostureDuration),
                    BadPostureTime = group.Where(p => !p.GoodPosture).Sum(p => p.PostureDuration)
                });

            return statistics;
        }

        public async Task<IEnumerable<PostureStatistics>> GetAllStatisticsAsync()
        {
            var postures = await _context.SittingPostures.ToListAsync();

            var statistics = postures
                .GroupBy(p => p.UserID)
                .Select(group => new PostureStatistics
                {
                    TotalSittingTime = group.Sum(p => p.PostureDuration),
                    GoodPostureTime = group.Where(p => p.GoodPosture).Sum(p => p.PostureDuration),
                    BadPostureTime = group.Where(p => !p.GoodPosture).Sum(p => p.PostureDuration)
                });

            return statistics;
        }
    }
}
