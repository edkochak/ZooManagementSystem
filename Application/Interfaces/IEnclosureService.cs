using Zoo.Application.DTOs;

namespace Zoo.Application.Interfaces
{
    public interface IEnclosureService
    {
        Task<IEnumerable<EnclosureDto>> GetAllEnclosuresAsync();
        Task<EnclosureDto> GetEnclosureByIdAsync(Guid id);
        Task<Guid> AddEnclosureAsync(EnclosureDto enclosureDto);
        Task UpdateEnclosureAsync(EnclosureDto enclosureDto);
        Task DeleteEnclosureAsync(Guid id);
        Task<IEnumerable<EnclosureDto>> GetAvailableEnclosuresAsync();
        Task<IEnumerable<AnimalDto>> GetAnimalsInEnclosureAsync(Guid enclosureId);
    }
}
