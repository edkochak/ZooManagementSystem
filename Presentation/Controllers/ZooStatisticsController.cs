using Microsoft.AspNetCore.Mvc;
using Zoo.Application.Services;

namespace Zoo.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZooStatisticsController : ControllerBase
    {
        private readonly ZooStatisticsService _statisticsService;
        
        public ZooStatisticsController(ZooStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        
        [HttpGet("animals/count")]
        public async Task<IActionResult> GetAnimalCount()
        {
            var count = await _statisticsService.GetTotalAnimalCountAsync();
            return Ok(new { TotalCount = count });
        }
        
        [HttpGet("animals/health")]
        public async Task<IActionResult> GetAnimalHealthStatistics()
        {
            var healthyCount = await _statisticsService.GetHealthyAnimalCountAsync();
            var sickCount = await _statisticsService.GetSickAnimalCountAsync();
            
            return Ok(new
            {
                HealthyCount = healthyCount,
                SickCount = sickCount,
                TotalCount = healthyCount + sickCount
            });
        }
        
        [HttpGet("animals/species")]
        public async Task<IActionResult> GetSpeciesStatistics()
        {
            var statistics = await _statisticsService.GetAnimalSpeciesStatisticsAsync();
            return Ok(statistics);
        }
        
        [HttpGet("enclosures/count")]
        public async Task<IActionResult> GetEnclosureCount()
        {
            var total = await _statisticsService.GetTotalEnclosuresAsync();
            var available = await _statisticsService.GetAvailableEnclosuresAsync();
            
            return Ok(new
            {
                TotalCount = total,
                AvailableCount = available,
                OccupiedCount = total - available
            });
        }
        
        [HttpGet("enclosures/types")]
        public async Task<IActionResult> GetEnclosureTypeStatistics()
        {
            var statistics = await _statisticsService.GetEnclosureTypeStatisticsAsync();
            return Ok(statistics);
        }
        
        [HttpGet("enclosures/occupancy")]
        public async Task<IActionResult> GetEnclosureOccupancyRate()
        {
            var rate = await _statisticsService.GetAverageEnclosureOccupancyAsync();
            return Ok(new { AverageOccupancyRate = $"{rate:P2}" });
        }
        
        [HttpGet("summary")]
        public async Task<IActionResult> GetZooSummary()
        {
            var animalCount = await _statisticsService.GetTotalAnimalCountAsync();
            var healthyCount = await _statisticsService.GetHealthyAnimalCountAsync();
            var enclosureCount = await _statisticsService.GetTotalEnclosuresAsync();
            var availableEnclosures = await _statisticsService.GetAvailableEnclosuresAsync();
            var occupancyRate = await _statisticsService.GetAverageEnclosureOccupancyAsync();
            
            return Ok(new
            {
                TotalAnimals = animalCount,
                HealthyAnimals = healthyCount,
                SickAnimals = animalCount - healthyCount,
                TotalEnclosures = enclosureCount,
                AvailableEnclosures = availableEnclosures,
                AverageOccupancy = $"{occupancyRate:P2}"
            });
        }
    }
}
