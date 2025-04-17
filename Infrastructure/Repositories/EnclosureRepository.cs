using Zoo.Domain.Entities;
using Zoo.Domain.Interfaces;
using Zoo.Infrastructure.Data;

namespace Zoo.Infrastructure.Repositories
{
    public class EnclosureRepository : IEnclosureRepository
    {
        private readonly InMemoryContext _context;
        
        public EnclosureRepository()
        {
            _context = InMemoryContext.Instance;
        }
        
        public Task<Enclosure> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_context.Enclosures.FirstOrDefault(e => e.Id == id));
        }
        
        public Task<IEnumerable<Enclosure>> GetAllAsync()
        {
            return Task.FromResult(_context.Enclosures.AsEnumerable());
        }
        
        public Task<IEnumerable<Enclosure>> GetAvailableEnclosuresAsync()
        {
            return Task.FromResult(_context.Enclosures.Where(e => e.CanAddAnimal()).AsEnumerable());
        }
        
        public Task AddAsync(Enclosure enclosure)
        {
            _context.Enclosures.Add(enclosure);
            return Task.CompletedTask;
        }
        
        public Task UpdateAsync(Enclosure enclosure)
        {
            // In a real database, we would update the entity
            // For in-memory, the object reference is already updated
            return Task.CompletedTask;
        }
        
        public Task DeleteAsync(Guid id)
        {
            var enclosure = _context.Enclosures.FirstOrDefault(e => e.Id == id);
            if (enclosure != null)
            {
                _context.Enclosures.Remove(enclosure);
            }
            return Task.CompletedTask;
        }
    }
}
