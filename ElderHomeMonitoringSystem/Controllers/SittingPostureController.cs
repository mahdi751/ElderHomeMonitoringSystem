using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElderHomeMonitoringSystem.Models;
using ElderHomeMonitoringSystem.Data;

namespace ElderHomeMonitoringSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SittingPostureController : ControllerBase
    {
        private readonly DataContext _context;

        public SittingPostureController(DataContext context)
        {
            _context = context;
        }

        // GET: api/SittingPosture
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SittingPosture>>> GetSittingPostures()
        {
            return await _context.SittingPostures.ToListAsync();
        }

        // GET: api/SittingPosture/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SittingPosture>> GetSittingPosture(int id)
        {
            var sittingPosture = await _context.SittingPostures.FindAsync(id);

            if (sittingPosture == null)
            {
                return NotFound();
            }

            return sittingPosture;
        }

        // POST: api/SittingPosture
        [HttpPost]
        public async Task<ActionResult<SittingPosture>> PostSittingPosture([FromBody]SittingPosture sittingPosture)
        {
            _context.SittingPostures.Add(sittingPosture);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSittingPosture", new { id = sittingPosture.Id }, sittingPosture);
        }

        // PUT: api/SittingPosture/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSittingPosture(int id, SittingPosture sittingPosture)
        {
            if (id != sittingPosture.Id)
            {
                return BadRequest();
            }

            _context.Entry(sittingPosture).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SittingPostureExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/SittingPosture/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSittingPosture(int id)
        {
            var sittingPosture = await _context.SittingPostures.FindAsync(id);
            if (sittingPosture == null)
            {
                return NotFound();
            }

            _context.SittingPostures.Remove(sittingPosture);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SittingPostureExists(int id)
        {
            return _context.SittingPostures.Any(e => e.Id == id);
        }
    }
}
