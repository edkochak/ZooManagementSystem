using Zoo.Domain.ValueObjects;
using Zoo.Domain.Events;

namespace Zoo.Domain.Entities
{
    public class FeedingSchedule : Entity
    {
        public Guid AnimalId { get; private set; }
        public FoodType FoodType { get; private set; }
        public TimeOnly FeedingTime { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateOnly ScheduleDate { get; private set; }
        
        // For EF Core
        private FeedingSchedule() : base() { }
        
        public FeedingSchedule(Guid animalId, FoodType foodType, TimeOnly feedingTime, DateOnly scheduleDate)
            : base()
        {
            AnimalId = animalId;
            FoodType = foodType ?? throw new ArgumentNullException(nameof(foodType));
            FeedingTime = feedingTime;
            ScheduleDate = scheduleDate;
        }
        
        public void ChangeFoodType(FoodType newFoodType)
        {
            FoodType = newFoodType ?? throw new ArgumentNullException(nameof(newFoodType));
        }
        
        public void ChangeTime(TimeOnly newFeedingTime)
        {
            FeedingTime = newFeedingTime;
            
            // Check if it's feeding time now and raise event if needed
            var now = TimeOnly.FromDateTime(DateTime.Now);
            if (ScheduleDate == DateOnly.FromDateTime(DateTime.Now) && 
                Math.Abs((now - FeedingTime).TotalMinutes) < 5)
            {
                DomainEvents.Raise(new FeedingTimeEvent(AnimalId, FoodType));
            }
        }
        
        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }
        
        public void ResetCompletion()
        {
            IsCompleted = false;
        }
    }
}
