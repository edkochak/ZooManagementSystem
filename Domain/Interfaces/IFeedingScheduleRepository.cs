using Zoo.Domain.Entities;

namespace Zoo.Domain.Interfaces
{
    public interface IFeedingScheduleRepository
    {
        Task<FeedingSchedule> GetByIdAsync(Guid id);
        Task<IEnumerable<FeedingSchedule>> GetAllAsync();
        Task<IEnumerable<FeedingSchedule>> GetByAnimalIdAsync(Guid animalId);
        Task<IEnumerable<FeedingSchedule>> GetByDateAsync(DateOnly date);
        Task AddAsync(FeedingSchedule schedule);
        Task UpdateAsync(FeedingSchedule schedule);
        Task DeleteAsync(Guid id);
    }
}
