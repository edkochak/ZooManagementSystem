using Zoo.Domain.Entities;
using Zoo.Domain.Interfaces;
using Zoo.Infrastructure.Data;

namespace Zoo.Infrastructure.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly InMemoryContext _context;
        
        public AnimalRepository()
        {
            _context = InMemoryContext.Instance;
        }
        
        public Task<Animal> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_context.Animals.FirstOrDefault(a => a.Id == id));
        }
        
        public Task<IEnumerable<Animal>> GetAllAsync()
        {
            return Task.FromResult(_context.Animals.AsEnumerable());
        }
        
        public Task AddAsync(Animal animal)
        {
            _context.Animals.Add(animal);
            return Task.CompletedTask;
        }
        
        public Task UpdateAsync(Animal animal)
        {
            // In a real database, we would update the entity
            // For in-memory, the object reference is already updated
            return Task.CompletedTask;
        }
        
        public Task DeleteAsync(Guid id)
        {
            var animal = _context.Animals.FirstOrDefault(a => a.Id == id);
            if (animal != null)
            {
                _context.Animals.Remove(animal);
            }
            return Task.CompletedTask;
        }
        
        public Task<IEnumerable<Animal>> GetByEnclosureIdAsync(Guid enclosureId)
        {
            return Task.FromResult(_context.Animals.Where(a => a.EnclosureId == enclosureId).AsEnumerable());
        }
    }
}
