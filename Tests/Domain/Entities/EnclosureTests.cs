using Xunit;
using Moq;

using System;
using Xunit;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Tests.Domain.Entities
{
    public class EnclosureTests
    {
        [Fact]
        public void Enclosure_Creation_ValidParameters_CreatesEnclosure()
        {
            // Arrange
            var type = EnclosureType.Predator;
            int size = 100;
            int maxCapacity = 5;

            // Act
            var enclosure = new Enclosure(type, size, maxCapacity);

            // Assert
            Assert.Equal(type, enclosure.Type);
            Assert.Equal(size, enclosure.Size);
            Assert.Equal(maxCapacity, enclosure.MaxCapacity);
            Assert.Empty(enclosure.Animals);
        }

        [Fact]
        public void Enclosure_Creation_NullType_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Enclosure(null, 100, 5));
        }

        [Fact]
        public void Enclosure_Creation_InvalidSize_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Enclosure(EnclosureType.Predator, 0, 5));
            Assert.Throws<ArgumentException>(() => new Enclosure(EnclosureType.Predator, -10, 5));
        }

        [Fact]
        public void Enclosure_Creation_InvalidCapacity_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Enclosure(EnclosureType.Predator, 100, 0));
            Assert.Throws<ArgumentException>(() => new Enclosure(EnclosureType.Predator, 100, -5));
        }

        [Fact]
        public void Enclosure_AddAnimal_ValidAnimal_ReturnsTrue()
        {
            // Arrange
            var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
            var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));

            // Act
            var result = enclosure.AddAnimal(animal);

            // Assert
            Assert.True(result);
            Assert.Single(enclosure.Animals);
            Assert.Equal(animal.Id, enclosure.Animals.First().Id);
            Assert.Equal(enclosure.Id, animal.EnclosureId);
        }

        [Fact]
        public void Enclosure_AddAnimal_NullAnimal_ThrowsException()
        {
            // Arrange
            var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => enclosure.AddAnimal(null));
        }

        [Fact]
        public void Enclosure_AddAnimal_ExceedCapacity_ReturnsFalse()
        {
            // Arrange
            var enclosure = new Enclosure(EnclosureType.Predator, 100, 1);
            var animal1 = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
            var animal2 = new Animal("Tiger", "Rajah", DateTime.Now.AddYears(-3), Gender.Male, FoodType.Create("Meat"));
            
            // Add first animal
            enclosure.AddAnimal(animal1);

            // Act - try to add second animal
            var result = enclosure.AddAnimal(animal2);

            // Assert
            Assert.False(result);
            Assert.Single(enclosure.Animals);
            Assert.Equal(animal1.Id, enclosure.Animals.First().Id);
        }

        [Fact]
        public void Enclosure_RemoveAnimal_ExistingAnimal_ReturnsTrue()
        {
            // Arrange
            var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
            var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
            enclosure.AddAnimal(animal);
            
            // Act
            var result = enclosure.RemoveAnimal(animal);
            
            // Assert
            Assert.True(result);
            Assert.Empty(enclosure.Animals);
            Assert.Null(animal.EnclosureId);
        }

        [Fact]
        public void Enclosure_RemoveAnimal_NonExistingAnimal_ReturnsFalse()
        {
            // Arrange
            var enclosure = new Enclosure(EnclosureType.Predator, 100, 5);
            var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
            
            // Act
            var result = enclosure.RemoveAnimal(animal);
            
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Enclosure_AddAnimal_WrongEnclosureType_ReturnsFalse()
        {
            // Arrange - create herbivore enclosure
            var enclosure = new Enclosure(EnclosureType.Herbivore, 100, 5);
            
            // Create a predator animal (lion)
            var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
            
            // Act - try to add predator to herbivore enclosure
            var result = enclosure.AddAnimal(animal);
            
            // Assert
            Assert.False(result);
            Assert.Empty(enclosure.Animals);
        }
    }
}
