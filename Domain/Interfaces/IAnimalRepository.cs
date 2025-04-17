using Zoo.Domain.Entities;

namespace Zoo.Domain.Interfaces
{
    public interface IAnimalRepository
    {
        Task<Animal> GetByIdAsync(Guid id);
        Task<IEnumerable<Animal>> GetAllAsync();
        Task AddAsync(Animal animal);
        Task UpdateAsync(Animal animal);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Animal>> GetByEnclosureIdAsync(Guid enclosureId);
    }
}
