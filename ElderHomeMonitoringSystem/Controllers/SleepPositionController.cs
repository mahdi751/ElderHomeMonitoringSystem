using ElderHomeMonitoringSystem.Data;
using ElderHomeMonitoringSystem.Interfaces;
using ElderHomeMonitoringSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;

namespace ElderHomeMonitoringSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SleepPositionController : ControllerBase
    {
        private readonly ISleepPositionRepository _sleepPositionRepository;
        private readonly ISleepSessionRepository _sleepSessionRepository;
        private readonly DataContext _context;
        private readonly IHubContext<AlertHub> _hubContext;


        public SleepPositionController(ISleepPositionRepository sleepPositionRepository, ISleepSessionRepository sleepSessionRepository, DataContext context, IHubContext<AlertHub> hubContext)
        {
            _sleepPositionRepository = sleepPositionRepository;
            _sleepSessionRepository = sleepSessionRepository;
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddSleepPositionAsync([FromBody] SleepPosition sleepPosition)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == sleepPosition.UserId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.Disability.Equals("Heart Failure") || user.Disability.Equals("Sleep Apnea") || user.Disability.Equals("Gastroesophageal Reflux Disease"))
            {
                if(sleepPosition.Position == "Supine")
                {
                    var connectedUsers = AlertHub.GetConnectedUsers();
                    Console.WriteLine($"Connected users: {string.Join(", ", connectedUsers)}");

                    var connectionIds = AlertHub.ConnectedUsers.Where(kvp => kvp.Value == user.UserID.ToString()).Select(kvp => kvp.Key).ToList();
                    if (connectionIds.Count > 0)
                    {
                        foreach (var connectionId in connectionIds)
                        {
                            await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", "Your Relative is sleeping in an uncomfortable position");
                        }
                    }
                    else
                    {
                        return NotFound($"User is not connected.");
                    }
                    _sleepPositionRepository.AddSleepPosition(sleepPosition);
                    return Ok("Sleep position added successfully. Disability discovered");
                }else
                {
                    _sleepPositionRepository.AddSleepPosition(sleepPosition);
                    return Ok("Sleep position added successfully. Disability discovered with no issue");
                }

            }
            else
            {
                _sleepPositionRepository.AddSleepPosition(sleepPosition);
                return Ok("Sleep position added successfully.");
            }
        }

        [HttpGet("most-common/{userId}")]
        public IActionResult GetMostCommonSleepPosition(int userId)
        {
            var mostCommonPosition = _sleepPositionRepository.GetMostCommonSleepPosition(userId);

            if (mostCommonPosition == null)
            {
                return NotFound("No sleep positions found for the user.");
            }

            return Ok(mostCommonPosition);
        }

        [HttpGet("most-common-interval/{userId}/{sleepSessionId}")]
        public async Task<IActionResult> GetMostCommonSleepPositionWithinIntervalAsync(int userId, int sleepSessionId)
        {
            var sleepSession =  await _sleepSessionRepository.GetSleepSessionByIdAsync(sleepSessionId);
            var mostCommonPosition = _sleepPositionRepository.GetMostCommonSleepPositionWithinInterval(userId, sleepSession.FromDate, sleepSession.ToDate);

            if (mostCommonPosition == "" || mostCommonPosition == null)
            {
                return NotFound("No sleep positions found for the user in the specified interval.");
            }

            return Ok(mostCommonPosition);
        }
    }
}
