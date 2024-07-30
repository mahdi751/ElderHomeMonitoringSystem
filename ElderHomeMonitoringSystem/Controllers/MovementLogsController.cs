using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ElderHomeMonitoringSystem.Interfaces;
using ElderHomeMonitoringSystem.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ElderHomeMonitoringSystem.Data;

namespace ElderHomeMonitoringSystem.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class MovementLogsController : Controller
    {
        private readonly IMovementRepository _repository;
        private readonly ISleepSessionRepository _sleepSessionRepository;
        private readonly DataContext _context;

        public MovementLogsController(IMovementRepository repository, ISleepSessionRepository sleepSessionRepository, DataContext context)
        {
            _repository = repository;
            _sleepSessionRepository = sleepSessionRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovementLog>>> GetMovementLogs()
        {
            var movementLogs = await _repository.GetAllAsync();
            return Ok(movementLogs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovementLog>> GetMovementLog(int id)
        {
            var movementLog = await _repository.GetByIdAsync(id);
            if (movementLog == null)
            {
                return NotFound();
            }

            return Ok(movementLog);
        }

        [HttpGet("GetByUser/{id}")]
        public async Task<ActionResult<IEnumerable<MovementLog>>> GetMovementByUserId(int id)
        {
            var movementLogs = await _repository.GetAllByUserIdAsync(id);
            if (movementLogs == null)
            {
                return NotFound();
            }

            return Ok(movementLogs);
        }

        [HttpPost]
        public async Task<ActionResult<MovementLog>> PostMovementLog([FromBody] MovementLog movementLog)
        {
            await _repository.AddAsync(movementLog);
            return CreatedAtAction(nameof(GetMovementLog), new { id = movementLog.Id }, movementLog);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovementLog(int id, MovementLog movementLog)
        {
            if (id != movementLog.Id)
            {
                return BadRequest();
            }

            await _repository.UpdateAsync(movementLog);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovementLog(int id)
        {
            var movementLog = await _repository.GetByIdAsync(id);
            if (movementLog == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("GetBySleepSession/{sleepSessionId}")]
        public async Task<ActionResult<IEnumerable<MovementLog>>> GetMovementsBySleepSession(int sleepSessionId)
        {
            var sleepSession = await _sleepSessionRepository.GetSleepSessionByIdAsync(sleepSessionId);
            if (sleepSession == null)
            {
                return NotFound();
            }

            var movementLogs = await _context.MovementLogs
                .Where(m => m.UserID == sleepSession.UserId && m.FromDate >= sleepSession.FromDate && m.ToDate <= sleepSession.ToDate)
                .ToListAsync();

            return Ok(movementLogs);
        }
    }
}
