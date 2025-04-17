using Zoo.Domain.Entities;
using Zoo.Domain.Events;
using Zoo.Domain.Interfaces;
using Zoo.Domain.ValueObjects;

namespace Zoo.Application.Services
{
    public class FeedingOrganizationService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IFeedingScheduleRepository _feedingScheduleRepository;

        public FeedingOrganizationService(
            IAnimalRepository animalRepository,
            IFeedingScheduleRepository feedingScheduleRepository)
        {
            _animalRepository = animalRepository;
            _feedingScheduleRepository = feedingScheduleRepository;
        }

        public async Task<Guid> ScheduleFeedingAsync(Guid animalId, string foodTypeName, TimeOnly feedingTime, DateOnly feedingDate)
        {
            var animal = await _animalRepository.GetByIdAsync(animalId);
            if (animal == null)
                throw new ArgumentException($"Animal with ID {animalId} not found");

            var foodType = FoodType.Create(foodTypeName);
            var schedule = new FeedingSchedule(animalId, foodType, feedingTime, feedingDate);
            
            await _feedingScheduleRepository.AddAsync(schedule);
            
            // Check if it's close to feeding time and trigger event if needed
            var now = TimeOnly.FromDateTime(DateTime.Now);
            if (feedingDate == DateOnly.FromDateTime(DateTime.Now) && 
                Math.Abs((now - feedingTime).TotalMinutes) < 5)
            {
                DomainEvents.Raise(new FeedingTimeEvent(animalId, foodType));
            }
            
            return schedule.Id;
        }

        public async Task MarkFeedingCompletedAsync(Guid scheduleId)
        {
            var schedule = await _feedingScheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
                throw new ArgumentException($"Feeding schedule with ID {scheduleId} not found");
                
            schedule.MarkAsCompleted();
            await _feedingScheduleRepository.UpdateAsync(schedule);
        }

        public async Task<IEnumerable<FeedingSchedule>> GetTodayFeedingSchedulesAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _feedingScheduleRepository.GetByDateAsync(today);
        }

        public async Task<IEnumerable<FeedingSchedule>> GetAnimalFeedingSchedulesAsync(Guid animalId)
        {
            return await _feedingScheduleRepository.GetByAnimalIdAsync(animalId);
        }
    }
}
