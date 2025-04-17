using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;
using Zoo.Domain.Events;
using Zoo.Domain.Interfaces;

namespace Zoo.Application.Services
{
    public class AnimalTransferService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IEnclosureRepository _enclosureRepository;

        public AnimalTransferService(
            IAnimalRepository animalRepository,
            IEnclosureRepository enclosureRepository)
        {
            _animalRepository = animalRepository;
            _enclosureRepository = enclosureRepository;
        }

        public async Task<bool> TransferAnimalAsync(Guid animalId, Guid targetEnclosureId)
        {
            var animal = await _animalRepository.GetByIdAsync(animalId);
            if (animal == null)
                return false;

            var targetEnclosure = await _enclosureRepository.GetByIdAsync(targetEnclosureId);
            if (targetEnclosure == null)
                return false;
                
            // Check if enclosure has space and is appropriate for the animal
            if (!targetEnclosure.CanAddAnimal())
                return false;

            // Transfer animal to new enclosure
            if (animal.EnclosureId.HasValue)
            {
                var currentEnclosure = await _enclosureRepository.GetByIdAsync(animal.EnclosureId.Value);
                if (currentEnclosure != null)
                {
                    currentEnclosure.RemoveAnimal(animal);
                    await _enclosureRepository.UpdateAsync(currentEnclosure);
                }
            }
            
            var result = targetEnclosure.AddAnimal(animal);
            
            if (result)
            {
                await _enclosureRepository.UpdateAsync(targetEnclosure);
                await _animalRepository.UpdateAsync(animal);
            }
            
            return result;
        }
        
        public async Task<bool> RemoveFromEnclosureAsync(Guid animalId)
        {
            var animal = await _animalRepository.GetByIdAsync(animalId);
            if (animal == null || !animal.EnclosureId.HasValue)
                return false;
                
            var currentEnclosure = await _enclosureRepository.GetByIdAsync(animal.EnclosureId.Value);
            if (currentEnclosure == null)
                return false;
                
            currentEnclosure.RemoveAnimal(animal);
            
            await _enclosureRepository.UpdateAsync(currentEnclosure);
            await _animalRepository.UpdateAsync(animal);
            
            return true;
        }
    }
}
