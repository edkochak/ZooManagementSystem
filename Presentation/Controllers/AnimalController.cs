using Microsoft.AspNetCore.Mvc;
using Zoo.Application.DTOs;
using Zoo.Application.Interfaces;

namespace Zoo.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalService _animalService;
        
        public AnimalController(IAnimalService animalService)
        {
            _animalService = animalService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var animals = await _animalService.GetAllAnimalsAsync();
            return Ok(animals);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var animal = await _animalService.GetAnimalByIdAsync(id);
            if (animal == null)
                return NotFound();
                
            return Ok(animal);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AnimalDto animalDto)
        {
            var id = await _animalService.AddAnimalAsync(animalDto);
            return CreatedAtAction(nameof(GetById), new { id }, animalDto);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AnimalDto animalDto)
        {
            if (id != animalDto.Id)
                return BadRequest("ID mismatch");
                
            await _animalService.UpdateAnimalAsync(animalDto);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _animalService.DeleteAnimalAsync(id);
            return NoContent();
        }
        
        [HttpPost("{animalId}/enclosures/{enclosureId}")]
        public async Task<IActionResult> MoveToEnclosure(Guid animalId, Guid enclosureId)
        {
            var result = await _animalService.MoveAnimalToEnclosureAsync(animalId, enclosureId);
            if (!result)
                return BadRequest("Could not move animal to specified enclosure");
                
            return NoContent();
        }
        
        [HttpDelete("{animalId}/enclosures")]
        public async Task<IActionResult> RemoveFromEnclosure(Guid animalId)
        {
            var result = await _animalService.RemoveAnimalFromEnclosureAsync(animalId);
            if (!result)
                return BadRequest("Could not remove animal from enclosure");
                
            return NoContent();
        }
    }
}
