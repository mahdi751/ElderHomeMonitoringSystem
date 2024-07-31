using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ElderHomeMonitoringSystem.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ElderHomeMonitoringSystem.Models;

namespace ElderHomeMonitoringSystem.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class SleepSessionsController : ControllerBase
    {
        private readonly ISleepSessionRepository _sleepSessionRepository;

        public SleepSessionsController(ISleepSessionRepository sleepSessionRepository)
        {
            _sleepSessionRepository = sleepSessionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SleepSession>>> GetAllSleepSessions()
        {
            var sleepSessions = await _sleepSessionRepository.GetAllSleepSessionsAsync();
            return Ok(sleepSessions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SleepSession>> GetSleepSessionById(int id)
        {
            var sleepSession = await _sleepSessionRepository.GetSleepSessionByIdAsync(id);
            if (sleepSession == null)
            {
                return NotFound();
            }
            return Ok(sleepSession);
        }

        [HttpPost]
        public async Task<ActionResult> AddSleepSession(SleepSession sleepSession)
        {
            await _sleepSessionRepository.AddSleepSessionAsync(sleepSession);
            return CreatedAtAction(nameof(GetSleepSessionById), new { id = sleepSession.Id }, sleepSession);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSleepSession(int id, SleepSession sleepSession)
        {
            if (id != sleepSession.Id)
            {
                return BadRequest();
            }

            var result = await _sleepSessionRepository.UpdateSleepSessionAsync(sleepSession);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSleepSession(int id)
        {
            var result = await _sleepSessionRepository.DeleteSleepSessionAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("range")]
        public async Task<ActionResult<IEnumerable<SleepSession>>> GetSleepSessionsByDateRange(DateTime startDate, DateTime endDate)
        {
            var sleepSessions = await _sleepSessionRepository.GetSleepSessionsByDateRangeAsync(startDate, endDate);
            return Ok(sleepSessions);
        }

        [HttpGet("last/{userId}")]
        public async Task<ActionResult<SleepSession>> GetLastSleepSession(int userId)
        {
            var sleepSessions = await _sleepSessionRepository.GetAllSleepSessionsAsync();
            var lastSleepSession = sleepSessions.Where(s => s.UserId == userId).OrderByDescending(s => s.ToDate).FirstOrDefault();
            if (lastSleepSession == null)
            {
                return NotFound();
            }
            return Ok(lastSleepSession);
        }
        [HttpGet("date/{userId}/{date}")]
        public async Task<ActionResult<SleepSession>> GetSleepSessionByDate(int userId, DateTime date)
        {
            var sleepSessions = await _sleepSessionRepository.GetAllSleepSessionsAsync();
            var sleepSession = sleepSessions.FirstOrDefault(s => s.UserId == userId && s.FromDate.Date == date.Date);
            if (sleepSession == null)
            {
                return NotFound();
            }
            return Ok(sleepSession);
        }

    }
}
