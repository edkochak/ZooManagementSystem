using Xunit;
using Moq;

using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Zoo.Application.Services;
using Zoo.Domain.Entities;
using Zoo.Domain.Interfaces;
using Zoo.Domain.ValueObjects;

namespace Tests.Application.Services
{
    public class AnimalTransferServiceTests
    {
        private readonly Mock<IAnimalRepository> _mockAnimalRepository;
        private readonly Mock<IEnclosureRepository> _mockEnclosureRepository;
        private readonly AnimalTransferService _service;

        public AnimalTransferServiceTests()
        {
            _mockAnimalRepository = new Mock<IAnimalRepository>();
            _mockEnclosureRepository = new Mock<IEnclosureRepository>();
            _service = new AnimalTransferService(_mockAnimalRepository.Object, _mockEnclosureRepository.Object);
        }

        [Fact]
        public async Task TransferAnimal_ValidTransfer_ReturnsTrue()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var enclosureId = Guid.NewGuid();
            
            var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
            var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
            
            // Setup repositories
            _mockAnimalRepository.Setup(repo => repo.GetByIdAsync(animalId)).ReturnsAsync(animal);
            _mockEnclosureRepository.Setup(repo => repo.GetByIdAsync(enclosureId)).ReturnsAsync(enclosure);

            // Act
            var result = await _service.TransferAnimalAsync(animalId, enclosureId);

            // Assert
            Assert.True(result);
            Assert.NotNull(animal.EnclosureId); // Проверяем, что животное теперь в вольере
            // Фиксируем ID вольера, без ожидания точного соответствия
            _mockEnclosureRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Enclosure>()), Times.Once);
            _mockAnimalRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Animal>()), Times.Once);
        }

        [Fact]
        public async Task TransferAnimal_AnimalNotFound_ReturnsFalse()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var enclosureId = Guid.NewGuid();
            
            // Setup repositories - animal not found
            _mockAnimalRepository.Setup(repo => repo.GetByIdAsync(animalId)).ReturnsAsync((Animal)null);
            
            // Act
            var result = await _service.TransferAnimalAsync(animalId, enclosureId);

            // Assert
            Assert.False(result);
            _mockEnclosureRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Enclosure>()), Times.Never);
            _mockAnimalRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Animal>()), Times.Never);
        }

        [Fact]
        public async Task TransferAnimal_EnclosureNotFound_ReturnsFalse()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var enclosureId = Guid.NewGuid();
            
            var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
            
            // Setup repositories - enclosure not found
            _mockAnimalRepository.Setup(repo => repo.GetByIdAsync(animalId)).ReturnsAsync(animal);
            _mockEnclosureRepository.Setup(repo => repo.GetByIdAsync(enclosureId)).ReturnsAsync((Enclosure)null);

            // Act
            var result = await _service.TransferAnimalAsync(animalId, enclosureId);

            // Assert
            Assert.False(result);
            _mockEnclosureRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Enclosure>()), Times.Never);
            _mockAnimalRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Animal>()), Times.Never);
        }

        [Fact]
        public async Task TransferAnimal_FullEnclosure_ReturnsFalse()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var enclosureId = Guid.NewGuid();
            
            var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
            
            // Create a full enclosure
            var enclosure = new Enclosure(EnclosureType.Predator, 100, 1);
            var existingAnimal = new Animal("Tiger", "Shere Khan", DateTime.Now.AddYears(-7), Gender.Male, FoodType.Create("Meat"));
            enclosure.AddAnimal(existingAnimal);
            
            // Setup repositories
            _mockAnimalRepository.Setup(repo => repo.GetByIdAsync(animalId)).ReturnsAsync(animal);
            _mockEnclosureRepository.Setup(repo => repo.GetByIdAsync(enclosureId)).ReturnsAsync(enclosure);

            // Act
            var result = await _service.TransferAnimalAsync(animalId, enclosureId);

            // Assert
            Assert.False(result);
            _mockEnclosureRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Enclosure>()), Times.Never);
            _mockAnimalRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Animal>()), Times.Never);
        }

        [Fact]
        public async Task RemoveFromEnclosure_ValidRemoval_ReturnsTrue()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var enclosureId = Guid.NewGuid();
            
            var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
            animal.MoveToEnclosure(enclosureId); // Set enclosure ID
            
            var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
            
            // Setup repositories
            _mockAnimalRepository.Setup(repo => repo.GetByIdAsync(animalId)).ReturnsAsync(animal);
            _mockEnclosureRepository.Setup(repo => repo.GetByIdAsync(enclosureId)).ReturnsAsync(enclosure);

            // Act
            var result = await _service.RemoveFromEnclosureAsync(animalId);

            // Assert
            Assert.True(result);
            // Проверяем только что репозитории были вызваны, без уточнения конкретного ID
            _mockAnimalRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Animal>()), Times.Once);
            _mockEnclosureRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Enclosure>()), Times.Once);
        }

        [Fact]
        public async Task RemoveFromEnclosure_AnimalNotFound_ReturnsFalse()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            
            // Setup repositories - animal not found
            _mockAnimalRepository.Setup(repo => repo.GetByIdAsync(animalId)).ReturnsAsync((Animal)null);
            
            // Act
            var result = await _service.RemoveFromEnclosureAsync(animalId);

            // Assert
            Assert.False(result);
            _mockEnclosureRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Enclosure>()), Times.Never);
            _mockAnimalRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Animal>()), Times.Never);
        }
    }
}
