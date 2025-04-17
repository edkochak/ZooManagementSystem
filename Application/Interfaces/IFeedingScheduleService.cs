using Zoo.Application.DTOs;

namespace Zoo.Application.Interfaces
{
    public interface IFeedingScheduleService
    {
        Task<IEnumerable<FeedingScheduleDto>> GetAllSchedulesAsync();
        Task<FeedingScheduleDto> GetScheduleByIdAsync(Guid id);
        Task<IEnumerable<FeedingScheduleDto>> GetSchedulesByAnimalIdAsync(Guid animalId);
        Task<IEnumerable<FeedingScheduleDto>> GetSchedulesByDateAsync(DateOnly date);
        Task<Guid> AddScheduleAsync(FeedingScheduleDto scheduleDto);
        Task UpdateScheduleAsync(FeedingScheduleDto scheduleDto);
        Task DeleteScheduleAsync(Guid id);
        Task MarkScheduleAsCompletedAsync(Guid id);
    }
}
