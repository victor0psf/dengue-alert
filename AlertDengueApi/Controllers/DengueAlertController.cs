using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AlertDengueApi.Interfaces;
using AlertDengueApi.Services;


namespace AlertDengueApi.Controllers
{
    [ApiController]
    [Route("api/dengue")]
    public class DengueAlertController : ControllerBase
    {
        private readonly IDengueAlertService _dengueAlertService;
        private readonly ILogger<DengueAlertController> _logger;

        public DengueAlertController(IDengueAlertService dengueAlertService, ILogger<DengueAlertController> logger)
        {
            _dengueAlertService = dengueAlertService;
            _logger = logger;
        }

        [HttpGet("sync")]
        public async Task<IActionResult> SyncDengueAlerts()
        {
            try
            {
                _logger.LogInformation("Starting dengue alerts synchronization.");
                var alerts = await _dengueAlertService.FetchAndSaveLastSixMonthsAsync();
                if (alerts == null || !alerts.Any())
                {
                    _logger.LogInformation("No new dengue alerts were found to synchronize.");
                    return NoContent();
                }

                return Ok(alerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while synchronizing dengue alerts.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{week}/{year}")]
        public async Task<IActionResult> GetAlertByWeekAndYear(int week, int year)
        {
            try
            {
                var alert = await _dengueAlertService.GetAlertByWeekAsync(week, year);
                if (alert == null)
                {
                    return NotFound();
                }

                return Ok(alert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving dengue alert.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAlerts()
        {
            try
            {
                var alerts = await _dengueAlertService.GetAllAlertsAsync();
                return Ok(alerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all dengue alerts.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}