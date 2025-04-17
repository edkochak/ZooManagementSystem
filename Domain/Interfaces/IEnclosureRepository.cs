using Zoo.Domain.Entities;

namespace Zoo.Domain.Interfaces
{
    public interface IEnclosureRepository
    {
        Task<Enclosure> GetByIdAsync(Guid id);
        Task<IEnumerable<Enclosure>> GetAllAsync();
        Task<IEnumerable<Enclosure>> GetAvailableEnclosuresAsync();
        Task AddAsync(Enclosure enclosure);
        Task UpdateAsync(Enclosure enclosure);
        Task DeleteAsync(Guid id);
    }
}
