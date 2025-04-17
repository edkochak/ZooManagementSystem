using Xunit;
using Moq;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Zoo.Application.Services;
using Zoo.Domain.Entities;
using Zoo.Domain.Interfaces;
using Zoo.Domain.ValueObjects;

namespace Tests.Application.Services
{
    public class ZooStatisticsServiceTests
    {
        private readonly Mock<IAnimalRepository> _mockAnimalRepository;
        private readonly Mock<IEnclosureRepository> _mockEnclosureRepository;
        private readonly ZooStatisticsService _service;

        public ZooStatisticsServiceTests()
        {
            _mockAnimalRepository = new Mock<IAnimalRepository>();
            _mockEnclosureRepository = new Mock<IEnclosureRepository>();
            _service = new ZooStatisticsService(_mockAnimalRepository.Object, _mockEnclosureRepository.Object);
        }

        [Fact]
        public async Task GetAnimalSpeciesStatistics_ReturnsCorrectStatistics()
        {
            // Arrange
            var animals = new List<Animal>
            {
                new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat")),
                new Animal("Lion", "Nala", DateTime.Now.AddYears(-4), Gender.Female, FoodType.Create("Meat")),
                new Animal("Tiger", "Shere Khan", DateTime.Now.AddYears(-7), Gender.Male, FoodType.Create("Meat")),
                new Animal("Giraffe", "Melman", DateTime.Now.AddYears(-6), Gender.Male, FoodType.Create("Leaves"))
            };
            
            _mockAnimalRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(animals);

            // Act
            var result = await _service.GetAnimalSpeciesStatisticsAsync();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(2, result["Lion"]);
            Assert.Equal(1, result["Tiger"]);
            Assert.Equal(1, result["Giraffe"]);
        }

        [Fact]
        public async Task GetTotalAnimalCount_ReturnsCorrectCount()
        {
            // Arrange
            var animals = new List<Animal>
            {
                new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat")),
                new Animal("Tiger", "Shere Khan", DateTime.Now.AddYears(-7), Gender.Male, FoodType.Create("Meat")),
                new Animal("Giraffe", "Melman", DateTime.Now.AddYears(-6), Gender.Male, FoodType.Create("Leaves"))
            };
            
            _mockAnimalRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(animals);

            // Act
            var result = await _service.GetTotalAnimalCountAsync();

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetHealthyAnimalCount_ReturnsCorrectCount()
        {
            // Arrange
            var animals = new List<Animal>
            {
                new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat")),
                new Animal("Tiger", "Shere Khan", DateTime.Now.AddYears(-7), Gender.Male, FoodType.Create("Meat")),
                new Animal("Giraffe", "Melman", DateTime.Now.AddYears(-6), Gender.Male, FoodType.Create("Leaves"))
            };
            
            // Mark one animal as sick
            animals[1].MarkAsSick();
            
            _mockAnimalRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(animals);

            // Act
            var result = await _service.GetHealthyAnimalCountAsync();

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetSickAnimalCount_ReturnsCorrectCount()
        {
            // Arrange
            var animals = new List<Animal>
            {
                new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat")),
                new Animal("Tiger", "Shere Khan", DateTime.Now.AddYears(-7), Gender.Male, FoodType.Create("Meat")),
                new Animal("Giraffe", "Melman", DateTime.Now.AddYears(-6), Gender.Male, FoodType.Create("Leaves"))
            };
            
            // Mark one animal as sick
            animals[1].MarkAsSick();
            
            _mockAnimalRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(animals);

            // Act
            var result = await _service.GetSickAnimalCountAsync();

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetTotalEnclosures_ReturnsCorrectCount()
        {
            // Arrange
            var enclosures = new List<Enclosure>
            {
                new Enclosure(EnclosureType.Predator, 100, 5),
                new Enclosure(EnclosureType.Herbivore, 200, 10),
                new Enclosure(EnclosureType.Aviary, 150, 20)
            };
            
            _mockEnclosureRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(enclosures);

            // Act
            var result = await _service.GetTotalEnclosuresAsync();

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetAvailableEnclosures_ReturnsCorrectCount()
        {
            // Arrange
            var enclosures = new List<Enclosure>
            {
                new Enclosure(EnclosureType.Predator, 100, 1),
                new Enclosure(EnclosureType.Herbivore, 200, 10),
                new Enclosure(EnclosureType.Aviary, 150, 20)
            };
            
            // Fill one enclosure to capacity
            var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
            enclosures[0].AddAnimal(animal);
            
            _mockEnclosureRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(enclosures);

            // Act
            var result = await _service.GetAvailableEnclosuresAsync();

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetEnclosureTypeStatistics_ReturnsCorrectStatistics()
        {
            // Arrange
            var enclosures = new List<Enclosure>
            {
                new Enclosure(EnclosureType.Predator, 100, 5),
                new Enclosure(EnclosureType.Predator, 120, 3),
                new Enclosure(EnclosureType.Herbivore, 200, 10),
                new Enclosure(EnclosureType.Aviary, 150, 20)
            };
            
            _mockEnclosureRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(enclosures);

            // Act
            var result = await _service.GetEnclosureTypeStatisticsAsync();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(2, result["Predator"]);
            Assert.Equal(1, result["Herbivore"]);
            Assert.Equal(1, result["Aviary"]);
        }

        [Fact]
        public async Task GetAverageEnclosureOccupancy_ReturnsCorrectAverage()
        {
            // Arrange
            var enclosures = new List<Enclosure>
            {
                new Enclosure(EnclosureType.Predator, 100, 4),
                new Enclosure(EnclosureType.Herbivore, 200, 10)
            };
            
            // Add animals to enclosures (2 to first, 5 to second)
            var predators = new[]
            {
                new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat")),
                new Animal("Tiger", "Shere Khan", DateTime.Now.AddYears(-7), Gender.Male, FoodType.Create("Meat"))
            };
            
            var herbivores = new[]
            {
                new Animal("Giraffe", "Melman", DateTime.Now.AddYears(-6), Gender.Male, FoodType.Create("Leaves")),
                new Animal("Zebra", "Marty", DateTime.Now.AddYears(-4), Gender.Male, FoodType.Create("Grass")),
                new Animal("Elephant", "Dumbo", DateTime.Now.AddYears(-2), Gender.Male, FoodType.Create("Peanuts")),
                new Animal("Hippo", "Gloria", DateTime.Now.AddYears(-5), Gender.Female, FoodType.Create("Fruits")),
                new Animal("Rhino", "Rocksteady", DateTime.Now.AddYears(-8), Gender.Male, FoodType.Create("Greens"))
            };
            
            foreach (var animal in predators)
            {
                enclosures[0].AddAnimal(animal);
            }
            
            foreach (var animal in herbivores)
            {
                enclosures[1].AddAnimal(animal);
            }
            
            _mockEnclosureRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(enclosures);

            // Act
            var result = await _service.GetAverageEnclosureOccupancyAsync();

            // Assert
            // Expected: (2/4 + 5/10) / 2 = (0.5 + 0.5) / 2 = 0.5
            Assert.Equal(0.5, result);
        }

        [Fact]
        public async Task GetAverageEnclosureOccupancy_NoEnclosures_ReturnsZero()
        {
            // Arrange
            _mockEnclosureRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Enclosure>());

            // Act
            var result = await _service.GetAverageEnclosureOccupancyAsync();

            // Assert
            Assert.Equal(0, result);
        }
    }
}
