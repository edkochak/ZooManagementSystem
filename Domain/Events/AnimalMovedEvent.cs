namespace Zoo.Domain.Events
{
    public class AnimalMovedEvent : DomainEvent
    {
        public Guid AnimalId { get; }
        public Guid? OldEnclosureId { get; }
        public Guid? NewEnclosureId { get; }
        
        public AnimalMovedEvent(Guid animalId, Guid? oldEnclosureId, Guid? newEnclosureId)
            : base()
        {
            AnimalId = animalId;
            OldEnclosureId = oldEnclosureId;
            NewEnclosureId = newEnclosureId;
        }
    }
}
