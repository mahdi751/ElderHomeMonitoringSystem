using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ElderHomeMonitoringSystem.Data;
using ElderHomeMonitoringSystem.Models;
using ElderHomeMonitoringSystem.Interfaces;

namespace ElderHomeMonitoringSystem.Repository
{
    public class SleepSessionRepository : ISleepSessionRepository
    {
        private readonly DataContext _context;

        public SleepSessionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<SleepSession>> GetAllSleepSessionsAsync()
        {
            return await _context.SleepSessions.ToListAsync();
        }

        public async Task<SleepSession> GetSleepSessionByIdAsync(int id)
        {
            return await _context.SleepSessions.FindAsync(id);
        }

        public async Task AddSleepSessionAsync(SleepSession sleepSession)
        {
            _context.SleepSessions.Add(sleepSession);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateSleepSessionAsync(SleepSession sleepSession)
        {
            if (!_context.SleepSessions.Any(s => s.Id == sleepSession.Id))
            {
                return false;
            }

            _context.Entry(sleepSession).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.SleepSessions.Any(s => s.Id == sleepSession.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteSleepSessionAsync(int id)
        {
            var sleepSession = await _context.SleepSessions.FindAsync(id);
            if (sleepSession == null)
            {
                return false;
            }

            _context.SleepSessions.Remove(sleepSession);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SleepSession>> GetSleepSessionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date must be less than or equal to end date");
            }

            var sleepSessions = await _context.SleepSessions
                .Where(s => s.FromDate >= startDate && s.ToDate <= endDate)
                .ToListAsync();

            return sleepSessions;
        }
    }
}
