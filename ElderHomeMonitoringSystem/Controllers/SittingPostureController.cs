using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElderHomeMonitoringSystem.Models;
using ElderHomeMonitoringSystem.Data;
using ElderHomeMonitoringSystem.Interfaces;
using ElderHomeMonitoringSystem.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Globalization;

namespace ElderHomeMonitoringSystem.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class SittingPostureController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ISittingPostureRepository _sittingPostureRepository;

        public SittingPostureController(DataContext context, ISittingPostureRepository sittingPostureRepository)
        {
            _context = context;
            _sittingPostureRepository = sittingPostureRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SittingPosture>>> GetSittingPostures()
        {
            return await _sittingPostureRepository.GetAllPosturesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SittingPosture>> GetSittingPosture(int id)
        {
            var sittingPosture = await _sittingPostureRepository.GetPostureByIdAsync(id);

            if (sittingPosture == null)
            {
                return NotFound();
            }

            return sittingPosture;
        }

        [HttpPost]
        public async Task<ActionResult<SittingPosture>> PostSittingPosture([FromBody] SittingPosture sittingPosture)
        {
            await _sittingPostureRepository.AddPostureAsync(sittingPosture);
            return CreatedAtAction("GetSittingPosture", new { id = sittingPosture.Id }, sittingPosture);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSittingPosture(int id, [FromBody] SittingPosture sittingPosture)
        {
            if (id != sittingPosture.Id)
            {
                return BadRequest();
            }

            try
            {
                await _sittingPostureRepository.UpdatePostureAsync(sittingPosture);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSittingPosture(int id)
        {
            var deleted = await _sittingPostureRepository.DeletePostureAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }


        [HttpPost("feedback")]
        public async Task<IActionResult> GetRealTimeFeedbackAsync([FromBody] SittingPosture latestData)
        {
            try
            {
                var feedback = await _sittingPostureRepository.ProvideFeedbackAsync(latestData);
                return Ok(new { Feedback = feedback });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to provide feedback", error = ex.Message });
            }
        }

        [HttpGet("daily-summary/{userId}")]
        public async Task<IActionResult> GetDailySummaryAsync(int userId)
        {
            try
            {
                var summary = await _sittingPostureRepository.GetDailySummaryAsync(userId);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error getting daily summary", error = ex.Message });
            }
        }

        [HttpGet("trends/{userId}")]
        public async Task<IActionResult> GetTrendsAsync(int userId)
        {
            try
            {
                var trends = await _sittingPostureRepository.GetTrendsAsync(userId);
                return Ok(trends);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error getting trends", error = ex.Message });
            }
        }
        [HttpGet("statistics/{userId}")]
        public async Task<ActionResult<PostureStatistics>> GetPostureStatistics(int userId)
        {
            return Ok(await _sittingPostureRepository.GetPostureStatisticsAsync(userId));
        }

        [HttpGet("statistics/{userId}/{period}")]
        public async Task<ActionResult<PostureStatistics>> GetFilteredPostureStatistics(int userId, string period)
        {
            return Ok(await _sittingPostureRepository.GetFilteredPostureStatisticsAsync(userId, period));
        }

        [HttpGet("statisticsByDate/{startDate}/{endDate}")]
        public async Task<IActionResult> GetPostureStatisticsByDateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                return BadRequest("End date must be greater than or equal to start date.");
            }

            try
            {
                var statistics = await _sittingPostureRepository.GetStatisticsByDateRangeAsync(startDate, endDate);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error processing request", error = ex.Message });
            }
        }


        [HttpGet("statistics")]
        public async Task<IActionResult> GetAllPostureStatistics()
        {
            try
            {
                var statistics = await _sittingPostureRepository.GetAllStatisticsAsync();
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error processing request", error = ex.Message });
            }
        }

        [HttpGet("GetPostureData/{startDate}/{endDate}")]
        public async Task<IActionResult> GetPostureData(DateTime startDate, DateTime endDate)
        {
            var postures = await _sittingPostureRepository.GetAll(startDate, endDate);
            return Ok(postures);
        }

        [HttpGet("daily/{date}/{UserId}")]
        public async Task<ActionResult<IEnumerable<SittingPosture>>> GetDailyData(string date,int UserId)
        {
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                return BadRequest("Invalid date format. Please use yyyy-MM-dd format.");

            var result = await _sittingPostureRepository.GetPosturesByDateAsync(parsedDate, UserId);
            return Ok(result);
        }

        [HttpGet("weekly/{startDate}/{UserId}")]
        public async Task<ActionResult<IEnumerable<SittingPosture>>> GetWeeklyData(string startDate, int UserId)
        {
            if (!DateTime.TryParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                return BadRequest("Invalid date format. Please use yyyy-MM-dd format.");

            var result = await _sittingPostureRepository.GetPosturesByWeekAsync(parsedDate, UserId);
            return Ok(result);
        }

        [HttpGet("monthly/{month}/{year}/{UserId}")]
        public async Task<ActionResult<IEnumerable<SittingPosture>>> GetMonthlyData(int month, int year, int UserId)
        {
            if (month < 1 || month > 12)
                return BadRequest("Month must be between 1 and 12.");

            var result = await _sittingPostureRepository.GetPosturesByMonthAsync(month, year, UserId);
            return Ok(result);
        }

        [HttpGet("daily-good-posture-percentage/{date}/{UserId}")]
        public async Task<IActionResult> GetDailyGoodPosturePercentage(string date, int UserId)
        {
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                return BadRequest("Invalid date format. Please use yyyy-MM-dd format.");

            var percentage = await _sittingPostureRepository.GetDailyGoodPosturePercentage(parsedDate, UserId);
            return Ok(percentage);
        }

        [HttpGet("weekly-good-posture-percentage/{startDate}/{UserId}")]
        public async Task<IActionResult> GetWeeklyGoodPosturePercentage(string startDate, int UserId)
        {
            if (!DateTime.TryParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                return BadRequest("Invalid date format. Please use yyyy-MM-dd format.");

            var percentage = await _sittingPostureRepository.GetWeeklyGoodPosturePercentage(parsedDate, UserId);
            return Ok(percentage);
        }

        [HttpGet("monthly-good-posture-percentage/{month}/{year}/{UserId}")]
        public async Task<IActionResult> GetMonthlyGoodPosturePercentage(int month, int year, int UserId)
        {
            if (month < 1 || month > 12)
                return BadRequest("Month must be between 1 and 12.");

            var percentage = await _sittingPostureRepository.GetMonthlyGoodPosturePercentage(month,year,UserId);
            return Ok(percentage);
        }
    }
}
