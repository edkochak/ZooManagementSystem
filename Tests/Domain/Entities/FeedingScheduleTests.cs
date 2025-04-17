using Xunit;
using Moq;

using System;
using Xunit;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

namespace Tests.Domain.Entities
{
    public class FeedingScheduleTests
    {
        [Fact]
        public void FeedingSchedule_Creation_ValidParameters_CreatesFeedingSchedule()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var foodType = FoodType.Create("Meat");
            var feedingTime = new TimeOnly(12, 0);
            var scheduleDate = DateOnly.FromDateTime(DateTime.Now);

            // Act
            var schedule = new FeedingSchedule(animalId, foodType, feedingTime, scheduleDate);

            // Assert
            Assert.Equal(animalId, schedule.AnimalId);
            Assert.Equal(foodType, schedule.FoodType);
            Assert.Equal(feedingTime, schedule.FeedingTime);
            Assert.Equal(scheduleDate, schedule.ScheduleDate);
            Assert.False(schedule.IsCompleted);
        }

        [Fact]
        public void FeedingSchedule_Creation_NullFoodType_ThrowsException()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            FoodType foodType = null;
            var feedingTime = new TimeOnly(12, 0);
            var scheduleDate = DateOnly.FromDateTime(DateTime.Now);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FeedingSchedule(animalId, foodType, feedingTime, scheduleDate));
        }

        [Fact]
        public void FeedingSchedule_ChangeFoodType_UpdatesFoodType()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var initialFoodType = FoodType.Create("Meat");
            var newFoodType = FoodType.Create("Fish");
            var feedingTime = new TimeOnly(12, 0);
            var scheduleDate = DateOnly.FromDateTime(DateTime.Now);

            var schedule = new FeedingSchedule(animalId, initialFoodType, feedingTime, scheduleDate);
            Assert.Equal(initialFoodType, schedule.FoodType);

            // Act
            schedule.ChangeFoodType(newFoodType);

            // Assert
            Assert.Equal(newFoodType, schedule.FoodType);
        }

        [Fact]
        public void FeedingSchedule_ChangeFoodType_NullFoodType_ThrowsException()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var foodType = FoodType.Create("Meat");
            var feedingTime = new TimeOnly(12, 0);
            var scheduleDate = DateOnly.FromDateTime(DateTime.Now);

            var schedule = new FeedingSchedule(animalId, foodType, feedingTime, scheduleDate);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => schedule.ChangeFoodType(null));
        }

        [Fact]
        public void FeedingSchedule_ChangeTime_UpdatesFeedingTime()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var foodType = FoodType.Create("Meat");
            var initialFeedingTime = new TimeOnly(12, 0);
            var newFeedingTime = new TimeOnly(14, 30);
            var scheduleDate = DateOnly.FromDateTime(DateTime.Now);

            var schedule = new FeedingSchedule(animalId, foodType, initialFeedingTime, scheduleDate);
            Assert.Equal(initialFeedingTime, schedule.FeedingTime);

            // Act
            schedule.ChangeTime(newFeedingTime);

            // Assert
            Assert.Equal(newFeedingTime, schedule.FeedingTime);
        }

        [Fact]
        public void FeedingSchedule_MarkAsCompleted_SetsIsCompletedToTrue()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var foodType = FoodType.Create("Meat");
            var feedingTime = new TimeOnly(12, 0);
            var scheduleDate = DateOnly.FromDateTime(DateTime.Now);

            var schedule = new FeedingSchedule(animalId, foodType, feedingTime, scheduleDate);
            Assert.False(schedule.IsCompleted);

            // Act
            schedule.MarkAsCompleted();

            // Assert
            Assert.True(schedule.IsCompleted);
        }

        [Fact]
        public void FeedingSchedule_ResetCompletion_SetsIsCompletedToFalse()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var foodType = FoodType.Create("Meat");
            var feedingTime = new TimeOnly(12, 0);
            var scheduleDate = DateOnly.FromDateTime(DateTime.Now);

            var schedule = new FeedingSchedule(animalId, foodType, feedingTime, scheduleDate);
            schedule.MarkAsCompleted();
            Assert.True(schedule.IsCompleted);

            // Act
            schedule.ResetCompletion();

            // Assert
            Assert.False(schedule.IsCompleted);
        }
    }
}
