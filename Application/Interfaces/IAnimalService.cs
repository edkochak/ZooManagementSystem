using Zoo.Application.DTOs;

namespace Zoo.Application.Interfaces
{
    public interface IAnimalService
    {
        Task<IEnumerable<AnimalDto>> GetAllAnimalsAsync();
        Task<AnimalDto> GetAnimalByIdAsync(Guid id);
        Task<Guid> AddAnimalAsync(AnimalDto animalDto);
        Task UpdateAnimalAsync(AnimalDto animalDto);
        Task DeleteAnimalAsync(Guid id);
        Task<bool> MoveAnimalToEnclosureAsync(Guid animalId, Guid enclosureId);
        Task<bool> RemoveAnimalFromEnclosureAsync(Guid animalId);
    }
}
