using Microsoft.AspNetCore.Mvc;
using Zoo.Application.DTOs;
using Zoo.Application.Interfaces;

namespace Zoo.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedingScheduleController : ControllerBase
    {
        private readonly IFeedingScheduleService _feedingScheduleService;
        
        public FeedingScheduleController(IFeedingScheduleService feedingScheduleService)
        {
            _feedingScheduleService = feedingScheduleService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var schedules = await _feedingScheduleService.GetAllSchedulesAsync();
            return Ok(schedules);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var schedule = await _feedingScheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
                return NotFound();
                
            return Ok(schedule);
        }
        
        [HttpGet("animal/{animalId}")]
        public async Task<IActionResult> GetByAnimalId(Guid animalId)
        {
            var schedules = await _feedingScheduleService.GetSchedulesByAnimalIdAsync(animalId);
            return Ok(schedules);
        }
        
        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetByDate(string date)
        {
            if (!DateOnly.TryParse(date, out var dateOnly))
                return BadRequest("Invalid date format. Use yyyy-MM-dd");
                
            var schedules = await _feedingScheduleService.GetSchedulesByDateAsync(dateOnly);
            return Ok(schedules);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeedingScheduleDto scheduleDto)
        {
            var id = await _feedingScheduleService.AddScheduleAsync(scheduleDto);
            return CreatedAtAction(nameof(GetById), new { id }, scheduleDto);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] FeedingScheduleDto scheduleDto)
        {
            if (id != scheduleDto.Id)
                return BadRequest("ID mismatch");
                
            await _feedingScheduleService.UpdateScheduleAsync(scheduleDto);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _feedingScheduleService.DeleteScheduleAsync(id);
            return NoContent();
        }
        
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> MarkAsCompleted(Guid id)
        {
            await _feedingScheduleService.MarkScheduleAsCompletedAsync(id);
            return NoContent();
        }
    }
}
