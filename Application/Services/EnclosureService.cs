using Zoo.Application.DTOs;
using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;
using Zoo.Domain.Interfaces;
using Zoo.Domain.ValueObjects;

namespace Zoo.Application.Services
{
    public class EnclosureService : IEnclosureService
    {
        private readonly IEnclosureRepository _enclosureRepository;
        private readonly IAnimalRepository _animalRepository;

        public EnclosureService(
            IEnclosureRepository enclosureRepository,
            IAnimalRepository animalRepository)
        {
            _enclosureRepository = enclosureRepository;
            _animalRepository = animalRepository;
        }

        public async Task<IEnumerable<EnclosureDto>> GetAllEnclosuresAsync()
        {
            var enclosures = await _enclosureRepository.GetAllAsync();
            return enclosures.Select(MapToDto);
        }

        public async Task<EnclosureDto> GetEnclosureByIdAsync(Guid id)
        {
            var enclosure = await _enclosureRepository.GetByIdAsync(id);
            return enclosure != null ? MapToDto(enclosure) : null;
        }

        public async Task<Guid> AddEnclosureAsync(EnclosureDto enclosureDto)
        {
            var enclosureType = EnclosureType.Create(enclosureDto.Type);
            
            var enclosure = new Enclosure(
                enclosureType,
                enclosureDto.Size,
                enclosureDto.MaxCapacity
            );
            
            await _enclosureRepository.AddAsync(enclosure);
            return enclosure.Id;
        }

        public async Task UpdateEnclosureAsync(EnclosureDto enclosureDto)
        {
            // In a real-world scenario, we would handle more complex updates
            // For now, this is a placeholder as enclosures don't have mutable properties in our model
            var enclosure = await _enclosureRepository.GetByIdAsync(enclosureDto.Id);
            if (enclosure == null)
                throw new ArgumentException($"Enclosure with ID {enclosureDto.Id} not found");
            
            // Any update logic would go here
            // For now, we just reload the enclosure
            await _enclosureRepository.UpdateAsync(enclosure);
        }

        public async Task DeleteEnclosureAsync(Guid id)
        {
            var enclosure = await _enclosureRepository.GetByIdAsync(id);
            if (enclosure == null)
                throw new ArgumentException($"Enclosure with ID {id} not found");
                
            // Check if there are animals in the enclosure
            if (enclosure.Animals.Any())
                throw new InvalidOperationException("Cannot delete enclosure that contains animals");
                
            await _enclosureRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<EnclosureDto>> GetAvailableEnclosuresAsync()
        {
            var enclosures = await _enclosureRepository.GetAvailableEnclosuresAsync();
            return enclosures.Select(MapToDto);
        }

        public async Task<IEnumerable<AnimalDto>> GetAnimalsInEnclosureAsync(Guid enclosureId)
        {
            var animals = await _animalRepository.GetByEnclosureIdAsync(enclosureId);
            return animals.Select(animal => new AnimalDto
            {
                Id = animal.Id,
                Species = animal.Species,
                Name = animal.Name,
                DateOfBirth = animal.DateOfBirth,
                Gender = animal.Gender.ToString(),
                FavoriteFood = animal.FavoriteFood.Name,
                IsHealthy = animal.IsHealthy,
                EnclosureId = animal.EnclosureId
            });
        }

        private EnclosureDto MapToDto(Enclosure enclosure)
        {
            return new EnclosureDto
            {
                Id = enclosure.Id,
                Type = enclosure.Type.Name,
                Size = enclosure.Size,
                MaxCapacity = enclosure.MaxCapacity,
                CurrentAnimalsCount = enclosure.Animals.Count,
                AnimalIds = enclosure.Animals.Select(a => a.Id).ToList()
            };
        }
    }
}
