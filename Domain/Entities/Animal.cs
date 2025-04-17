using Zoo.Domain.ValueObjects;
using Zoo.Domain.Events;

namespace Zoo.Domain.Entities
{
    public class Animal : Entity
    {
        public string Species { get; private set; }
        public string Name { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Gender Gender { get; private set; }
        public FoodType FavoriteFood { get; private set; }
        public bool IsHealthy { get; private set; } = true;
        public Guid? EnclosureId { get; private set; }

        // For EF Core
        private Animal() : base() { }

        public Animal(string species, string name, DateTime dateOfBirth, Gender gender, FoodType favoriteFood)
            : base()
        {
            if (string.IsNullOrWhiteSpace(species))
                throw new ArgumentException("Species cannot be empty", nameof(species));
                
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
                
            if (dateOfBirth > DateTime.Now)
                throw new ArgumentException("Date of birth cannot be in the future", nameof(dateOfBirth));
                
            Species = species;
            Name = name;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            FavoriteFood = favoriteFood ?? throw new ArgumentNullException(nameof(favoriteFood));
        }

        public void Feed()
        {
            // Implementation of feeding the animal
            // Could trigger domain events or change state
        }

        public void Treat()
        {
            if (!IsHealthy)
            {
                IsHealthy = true;
            }
        }

        public void MarkAsSick()
        {
            if (IsHealthy)
            {
                IsHealthy = false;
            }
        }

        public void MoveToEnclosure(Guid enclosureId)
        {
            var oldEnclosureId = EnclosureId;
            EnclosureId = enclosureId;
            
            // Raise domain event
            DomainEvents.Raise(new AnimalMovedEvent(Id, oldEnclosureId, enclosureId));
        }

        public void RemoveFromEnclosure()
        {
            if (EnclosureId.HasValue)
            {
                var oldEnclosureId = EnclosureId;
                EnclosureId = null;
                
                // Raise domain event
                DomainEvents.Raise(new AnimalMovedEvent(Id, oldEnclosureId, null));
            }
        }
    }
}
