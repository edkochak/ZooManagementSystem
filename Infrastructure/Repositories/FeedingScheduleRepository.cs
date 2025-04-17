using Zoo.Domain.Entities;
using Zoo.Domain.Interfaces;
using Zoo.Infrastructure.Data;

namespace Zoo.Infrastructure.Repositories
{
    public class FeedingScheduleRepository : IFeedingScheduleRepository
    {
        private readonly InMemoryContext _context;
        
        public FeedingScheduleRepository()
        {
            _context = InMemoryContext.Instance;
        }
        
        public Task<FeedingSchedule> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_context.FeedingSchedules.FirstOrDefault(f => f.Id == id));
        }
        
        public Task<IEnumerable<FeedingSchedule>> GetAllAsync()
        {
            return Task.FromResult(_context.FeedingSchedules.AsEnumerable());
        }
        
        public Task<IEnumerable<FeedingSchedule>> GetByAnimalIdAsync(Guid animalId)
        {
            return Task.FromResult(_context.FeedingSchedules.Where(f => f.AnimalId == animalId).AsEnumerable());
        }
        
        public Task<IEnumerable<FeedingSchedule>> GetByDateAsync(DateOnly date)
        {
            return Task.FromResult(_context.FeedingSchedules.Where(f => f.ScheduleDate == date).AsEnumerable());
        }
        
        public Task AddAsync(FeedingSchedule schedule)
        {
            _context.FeedingSchedules.Add(schedule);
            return Task.CompletedTask;
        }
        
        public Task UpdateAsync(FeedingSchedule schedule)
        {
            // In a real database, we would update the entity
            // For in-memory, the object reference is already updated
            return Task.CompletedTask;
        }
        
        public Task DeleteAsync(Guid id)
        {
            var schedule = _context.FeedingSchedules.FirstOrDefault(f => f.Id == id);
            if (schedule != null)
            {
                _context.FeedingSchedules.Remove(schedule);
            }
            return Task.CompletedTask;
        }
    }
}
