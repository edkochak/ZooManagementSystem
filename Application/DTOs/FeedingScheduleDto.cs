namespace Zoo.Application.DTOs
{
    public class FeedingScheduleDto
    {
        public Guid Id { get; set; }
        public Guid AnimalId { get; set; }
        public string FoodType { get; set; }
        public TimeOnly FeedingTime { get; set; }
        public DateOnly ScheduleDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
