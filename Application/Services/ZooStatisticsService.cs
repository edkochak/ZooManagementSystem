using Zoo.Domain.Interfaces;

namespace Zoo.Application.Services
{
    public class ZooStatisticsService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IEnclosureRepository _enclosureRepository;

        public ZooStatisticsService(
            IAnimalRepository animalRepository,
            IEnclosureRepository enclosureRepository)
        {
            _animalRepository = animalRepository;
            _enclosureRepository = enclosureRepository;
        }

        public async Task<IDictionary<string, int>> GetAnimalSpeciesStatisticsAsync()
        {
            var animals = await _animalRepository.GetAllAsync();
            return animals
                .GroupBy(a => a.Species)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<int> GetTotalAnimalCountAsync()
        {
            var animals = await _animalRepository.GetAllAsync();
            return animals.Count();
        }

        public async Task<int> GetHealthyAnimalCountAsync()
        {
            var animals = await _animalRepository.GetAllAsync();
            return animals.Count(a => a.IsHealthy);
        }

        public async Task<int> GetSickAnimalCountAsync()
        {
            var animals = await _animalRepository.GetAllAsync();
            return animals.Count(a => !a.IsHealthy);
        }

        public async Task<int> GetTotalEnclosuresAsync()
        {
            var enclosures = await _enclosureRepository.GetAllAsync();
            return enclosures.Count();
        }

        public async Task<int> GetAvailableEnclosuresAsync()
        {
            var enclosures = await _enclosureRepository.GetAllAsync();
            return enclosures.Count(e => e.Animals.Count < e.MaxCapacity);
        }

        public async Task<IDictionary<string, int>> GetEnclosureTypeStatisticsAsync()
        {
            var enclosures = await _enclosureRepository.GetAllAsync();
            return enclosures
                .GroupBy(e => e.Type.Name)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<double> GetAverageEnclosureOccupancyAsync()
        {
            var enclosures = await _enclosureRepository.GetAllAsync();
            if (!enclosures.Any())
                return 0;
                
            return enclosures.Average(e => (double)e.Animals.Count / e.MaxCapacity);
        }
    }
}
