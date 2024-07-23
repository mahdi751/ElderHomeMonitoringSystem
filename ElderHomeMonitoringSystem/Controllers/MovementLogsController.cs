using ElderHomeMonitoringSystem.Interfaces;
using ElderHomeMonitoringSystem.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ElderHomeMonitoringSystem.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class MovementLogsController : Controller
    {
        private readonly IMovementRepository _repository;

        public MovementLogsController(IMovementRepository repository)
        {
            _repository = repository;
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
        public async Task<ActionResult<MovementLog>> GetMovementByUserId(int id)
        {
            var movementLog = await _repository.GetAllByUserIdAsync(id);
            if (movementLog == null)
            {
                return NotFound();
            }

            return Ok(movementLog);
        }


        [HttpPost]
        public async Task<ActionResult<MovementLog>> PostMovementLog([FromBody]MovementLog movementLog)
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
    }
}
