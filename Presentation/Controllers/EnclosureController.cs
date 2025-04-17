using Microsoft.AspNetCore.Mvc;
using Zoo.Application.DTOs;
using Zoo.Application.Interfaces;

namespace Zoo.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnclosureController : ControllerBase
    {
        private readonly IEnclosureService _enclosureService;
        
        public EnclosureController(IEnclosureService enclosureService)
        {
            _enclosureService = enclosureService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var enclosures = await _enclosureService.GetAllEnclosuresAsync();
            return Ok(enclosures);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var enclosure = await _enclosureService.GetEnclosureByIdAsync(id);
            if (enclosure == null)
                return NotFound();
                
            return Ok(enclosure);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EnclosureDto enclosureDto)
        {
            var id = await _enclosureService.AddEnclosureAsync(enclosureDto);
            return CreatedAtAction(nameof(GetById), new { id }, enclosureDto);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EnclosureDto enclosureDto)
        {
            if (id != enclosureDto.Id)
                return BadRequest("ID mismatch");
                
            await _enclosureService.UpdateEnclosureAsync(enclosureDto);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try 
            {
                await _enclosureService.DeleteEnclosureAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var enclosures = await _enclosureService.GetAvailableEnclosuresAsync();
            return Ok(enclosures);
        }
        
        [HttpGet("{id}/animals")]
        public async Task<IActionResult> GetAnimalsInEnclosure(Guid id)
        {
            var animals = await _enclosureService.GetAnimalsInEnclosureAsync(id);
            return Ok(animals);
        }
    }
}
