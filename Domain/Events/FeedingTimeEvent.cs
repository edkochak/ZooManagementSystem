using Zoo.Domain.ValueObjects;

namespace Zoo.Domain.Events
{
    public class FeedingTimeEvent : DomainEvent
    {
        public Guid AnimalId { get; }
        public FoodType FoodType { get; }
        
        public FeedingTimeEvent(Guid animalId, FoodType foodType)
            : base()
        {
            AnimalId = animalId;
            FoodType = foodType;
        }
    }
}
