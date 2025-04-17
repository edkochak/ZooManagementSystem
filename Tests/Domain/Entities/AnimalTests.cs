using Xunit;
using Moq;

using System;
using Xunit;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Tests.Domain.Entities
{
    public class AnimalTests
    {
        [Fact]
        public void Animal_Creation_ValidParameters_CreatesAnimal()
        {
            // Arrange
            string species = "Lion";
            string name = "Simba";
            DateTime dateOfBirth = DateTime.Now.AddYears(-5);
            Gender gender = Gender.Male;
            FoodType favoriteFood = FoodType.Create("Meat");

            // Act
            var animal = new Animal(species, name, dateOfBirth, gender, favoriteFood);

            // Assert
            Assert.Equal(species, animal.Species);
            Assert.Equal(name, animal.Name);
            Assert.Equal(dateOfBirth, animal.DateOfBirth);
            Assert.Equal(gender, animal.Gender);
            Assert.Equal(favoriteFood, animal.FavoriteFood);
            Assert.True(animal.IsHealthy);
            Assert.Null(animal.EnclosureId);
        }

        [Fact]
        public void Animal_Creation_InvalidSpecies_ThrowsException()
        {
            // Arrange
            string species = "";
            string name = "Simba";
            DateTime dateOfBirth = DateTime.Now.AddYears(-5);
            Gender gender = Gender.Male;
            FoodType favoriteFood = FoodType.Create("Meat");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Animal(species, name, dateOfBirth, gender, favoriteFood));
        }

        [Fact]
        public void Animal_Creation_InvalidName_ThrowsException()
        {
            // Arrange
            string species = "Lion";
            string name = "";
            DateTime dateOfBirth = DateTime.Now.AddYears(-5);
            Gender gender = Gender.Male;
            FoodType favoriteFood = FoodType.Create("Meat");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Animal(species, name, dateOfBirth, gender, favoriteFood));
        }

        [Fact]
        public void Animal_Creation_FutureBirthDate_ThrowsException()
        {
            // Arrange
            string species = "Lion";
            string name = "Simba";
            DateTime dateOfBirth = DateTime.Now.AddYears(1); // Future date
            Gender gender = Gender.Male;
            FoodType favoriteFood = FoodType.Create("Meat");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Animal(species, name, dateOfBirth, gender, favoriteFood));
        }

        [Fact]
        public void Animal_MarkAsSick_SetsHealthStatusToFalse()
        {
            // Arrange
            var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
            
            // Act
            animal.MarkAsSick();
            
            // Assert
            Assert.False(animal.IsHealthy);
        }

        [Fact]
        public void Animal_Treat_SetsHealthStatusToTrue()
        {
            // Arrange
            var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
            animal.MarkAsSick();
            Assert.False(animal.IsHealthy);
            
            // Act
            animal.Treat();
            
            // Assert
            Assert.True(animal.IsHealthy);
        }

        [Fact]
        public void Animal_MoveToEnclosure_UpdatesEnclosureId()
        {
            // Arrange
            var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
            var enclosureId = Guid.NewGuid();
            
            // Act
            animal.MoveToEnclosure(enclosureId);
            
            // Assert
            Assert.Equal(enclosureId, animal.EnclosureId);
        }

        [Fact]
        public void Animal_RemoveFromEnclosure_SetsEnclosureIdToNull()
        {
            // Arrange
            var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
            animal.MoveToEnclosure(Guid.NewGuid());
            Assert.NotNull(animal.EnclosureId);
            
            // Act
            animal.RemoveFromEnclosure();
            
            // Assert
            Assert.Null(animal.EnclosureId);
        }
    }
}
