using Xunit;
using Moq;

using System;
using Xunit;
using Zoo.Domain.ValueObjects;

namespace Tests.Domain.ValueObjects
{
    public class ValueObjectTests
    {
        [Fact]
        public void FoodType_Creation_ValidName_CreatesFoodType()
        {
            // Arrange & Act
            var foodType = FoodType.Create("Meat");
            
            // Assert
            Assert.Equal("Meat", foodType.Name);
        }
        
        [Fact]
        public void FoodType_Creation_EmptyName_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => FoodType.Create(""));
            Assert.Throws<ArgumentException>(() => FoodType.Create(" "));
            Assert.Throws<ArgumentException>(() => FoodType.Create(null));
        }
        
        [Fact]
        public void FoodType_Equals_SameName_ReturnsTrue()
        {
            // Arrange
            var foodType1 = FoodType.Create("Meat");
            var foodType2 = FoodType.Create("Meat");
            
            // Act & Assert
            Assert.Equal(foodType1, foodType2);
            Assert.True(foodType1.Equals(foodType2));
            Assert.Equal(foodType1.GetHashCode(), foodType2.GetHashCode());
        }
        
        [Fact]
        public void FoodType_Equals_DifferentNames_ReturnsFalse()
        {
            // Arrange
            var foodType1 = FoodType.Create("Meat");
            var foodType2 = FoodType.Create("Fish");
            
            // Act & Assert
            Assert.NotEqual(foodType1, foodType2);
            Assert.False(foodType1.Equals(foodType2));
            Assert.NotEqual(foodType1.GetHashCode(), foodType2.GetHashCode());
        }
        
        [Fact]
        public void FoodType_ToString_ReturnsName()
        {
            // Arrange
            var foodType = FoodType.Create("Meat");
            
            // Act
            var result = foodType.ToString();
            
            // Assert
            Assert.Equal("Meat", result);
        }
        
        [Fact]
        public void EnclosureType_Creation_ValidName_CreatesEnclosureType()
        {
            // Arrange & Act
            var enclosureType = EnclosureType.Create("Custom");
            
            // Assert
            Assert.Equal("Custom", enclosureType.Name);
        }
        
        [Fact]
        public void EnclosureType_PredefinedTypes_ReturnCorrectValues()
        {
            // Act & Assert
            Assert.Equal("Predator", EnclosureType.Predator.Name);
            Assert.Equal("Herbivore", EnclosureType.Herbivore.Name);
            Assert.Equal("Aviary", EnclosureType.Aviary.Name);
            Assert.Equal("Aquarium", EnclosureType.Aquarium.Name);
        }
        
        [Fact]
        public void EnclosureType_Creation_EmptyName_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => EnclosureType.Create(""));
            Assert.Throws<ArgumentException>(() => EnclosureType.Create(" "));
            Assert.Throws<ArgumentException>(() => EnclosureType.Create(null));
        }
        
        [Fact]
        public void EnclosureType_Equals_SameName_ReturnsTrue()
        {
            // Arrange
            var type1 = EnclosureType.Create("Custom");
            var type2 = EnclosureType.Create("Custom");
            
            // Act & Assert
            Assert.Equal(type1, type2);
            Assert.True(type1.Equals(type2));
            Assert.Equal(type1.GetHashCode(), type2.GetHashCode());
        }
        
        [Fact]
        public void EnclosureType_Equals_DifferentNames_ReturnsFalse()
        {
            // Arrange
            var type1 = EnclosureType.Create("Custom1");
            var type2 = EnclosureType.Create("Custom2");
            
            // Act & Assert
            Assert.NotEqual(type1, type2);
            Assert.False(type1.Equals(type2));
            Assert.NotEqual(type1.GetHashCode(), type2.GetHashCode());
        }
    }
}
