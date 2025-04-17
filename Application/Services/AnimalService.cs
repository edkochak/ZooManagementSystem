using Zoo.Application.DTOs;
using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;
using Zoo.Domain.Interfaces;
using Zoo.Domain.ValueObjects;

namespace Zoo.Application.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly AnimalTransferService _animalTransferService;

        public AnimalService(
            IAnimalRepository animalRepository,
            AnimalTransferService animalTransferService)
        {
            _animalRepository = animalRepository;
            _animalTransferService = animalTransferService;
        }

        public async Task<IEnumerable<AnimalDto>> GetAllAnimalsAsync()
        {
            var animals = await _animalRepository.GetAllAsync();
            return animals.Select(MapToDto);
        }

        public async Task<AnimalDto> GetAnimalByIdAsync(Guid id)
        {
            var animal = await _animalRepository.GetByIdAsync(id);
            return animal != null ? MapToDto(animal) : null;
        }

        public async Task<Guid> AddAnimalAsync(AnimalDto animalDto)
        {
            var gender = Enum.Parse<Gender>(animalDto.Gender);
            var favoriteFood = FoodType.Create(animalDto.FavoriteFood);
            
            var animal = new Animal(
                animalDto.Species,
                animalDto.Name,
                animalDto.DateOfBirth,
                gender,
                favoriteFood
            );
            
            await _animalRepository.AddAsync(animal);
            return animal.Id;
        }

        public async Task UpdateAnimalAsync(AnimalDto animalDto)
        {
            var animal = await _animalRepository.GetByIdAsync(animalDto.Id);
            if (animal == null)
                throw new ArgumentException($"Animal with ID {animalDto.Id} not found");
                
            // In a real-world scenario, we would update the animal's properties
            // For now, we'll just handle the healthy status
            if (animalDto.IsHealthy && !animal.IsHealthy)
            {
                animal.Treat();
            }
            else if (!animalDto.IsHealthy && animal.IsHealthy)
            {
                animal.MarkAsSick();
            }
            
            await _animalRepository.UpdateAsync(animal);
        }

        public async Task DeleteAnimalAsync(Guid id)
        {
            await _animalRepository.DeleteAsync(id);
        }

        public async Task<bool> MoveAnimalToEnclosureAsync(Guid animalId, Guid enclosureId)
        {
            return await _animalTransferService.TransferAnimalAsync(animalId, enclosureId);
        }

        public async Task<bool> RemoveAnimalFromEnclosureAsync(Guid animalId)
        {
            return await _animalTransferService.RemoveFromEnclosureAsync(animalId);
        }

        private AnimalDto MapToDto(Animal animal)
        {
            return new AnimalDto
            {
                Id = animal.Id,
                Species = animal.Species,
                Name = animal.Name,
                DateOfBirth = animal.DateOfBirth,
                Gender = animal.Gender.ToString(),
                FavoriteFood = animal.FavoriteFood.Name,
                IsHealthy = animal.IsHealthy,
                EnclosureId = animal.EnclosureId
            };
        }
    }
}
