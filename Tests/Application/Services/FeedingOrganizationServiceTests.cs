using Xunit;
using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Zoo.Application.Services;
using Zoo.Domain.Entities;
using Zoo.Domain.Interfaces;
using Zoo.Domain.ValueObjects;

namespace Tests.Application.Services
{
    public class FeedingOrganizationServiceTests
    {
        private readonly Mock<IAnimalRepository> _mockAnimalRepository;
        private readonly Mock<IFeedingScheduleRepository> _mockFeedingScheduleRepository;
        private readonly FeedingOrganizationService _service;

        public FeedingOrganizationServiceTests()
        {
            _mockAnimalRepository = new Mock<IAnimalRepository>();
            _mockFeedingScheduleRepository = new Mock<IFeedingScheduleRepository>();
            _service = new FeedingOrganizationService(_mockAnimalRepository.Object, _mockFeedingScheduleRepository.Object);
        }

        [Fact]
        public async Task ScheduleFeeding_ValidParameters_ReturnsNewId()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var foodTypeName = "Meat";
            var feedingTime = new TimeOnly(12, 0);
            var feedingDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            
            var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
            
            // Setup repositories
            _mockAnimalRepository.Setup(repo => repo.GetByIdAsync(animalId)).ReturnsAsync(animal);
            _mockFeedingScheduleRepository
                .Setup(repo => repo.AddAsync(It.IsAny<FeedingSchedule>()))
                .Returns(Task.CompletedTask)
                .Callback<FeedingSchedule>(schedule => 
                {
                    Assert.Equal(animalId, schedule.AnimalId);
                    Assert.Equal(foodTypeName, schedule.FoodType.Name);
                    Assert.Equal(feedingTime, schedule.FeedingTime);
                    Assert.Equal(feedingDate, schedule.ScheduleDate);
                });

            // Act
            var result = await _service.ScheduleFeedingAsync(animalId, foodTypeName, feedingTime, feedingDate);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            _mockFeedingScheduleRepository.Verify(repo => repo.AddAsync(It.IsAny<FeedingSchedule>()), Times.Once);
        }

        [Fact]
        public async Task ScheduleFeeding_AnimalNotFound_ThrowsException()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var foodTypeName = "Meat";
            var feedingTime = new TimeOnly(12, 0);
            var feedingDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            
            // Setup repositories - animal not found
            _mockAnimalRepository.Setup(repo => repo.GetByIdAsync(animalId)).ReturnsAsync((Animal)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _service.ScheduleFeedingAsync(animalId, foodTypeName, feedingTime, feedingDate));
            
            _mockFeedingScheduleRepository.Verify(repo => repo.AddAsync(It.IsAny<FeedingSchedule>()), Times.Never);
        }

        [Fact]
        public async Task MarkFeedingCompleted_ValidId_MarksAsCompleted()
        {
            // Arrange
            var scheduleId = Guid.NewGuid();
            var schedule = new FeedingSchedule(
                Guid.NewGuid(), 
                FoodType.Create("Meat"), 
                new TimeOnly(12, 0), 
                DateOnly.FromDateTime(DateTime.Now)
            );
            Assert.False(schedule.IsCompleted);
            
            // Setup repositories
            _mockFeedingScheduleRepository.Setup(repo => repo.GetByIdAsync(scheduleId)).ReturnsAsync(schedule);

            // Act
            await _service.MarkFeedingCompletedAsync(scheduleId);

            // Assert
            Assert.True(schedule.IsCompleted);
            _mockFeedingScheduleRepository.Verify(repo => repo.UpdateAsync(schedule), Times.Once);
        }

        [Fact]
        public async Task MarkFeedingCompleted_ScheduleNotFound_ThrowsException()
        {
            // Arrange
            var scheduleId = Guid.NewGuid();
            
            // Setup repositories - schedule not found
            _mockFeedingScheduleRepository.Setup(repo => repo.GetByIdAsync(scheduleId)).ReturnsAsync((FeedingSchedule)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.MarkFeedingCompletedAsync(scheduleId));
            _mockFeedingScheduleRepository.Verify(repo => repo.UpdateAsync(It.IsAny<FeedingSchedule>()), Times.Never);
        }

        [Fact]
        public async Task GetTodayFeedingSchedules_ReturnsCorrectSchedules()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Now);
            var schedules = new List<FeedingSchedule>
            {
                new FeedingSchedule(Guid.NewGuid(), FoodType.Create("Meat"), new TimeOnly(9, 0), today),
                new FeedingSchedule(Guid.NewGuid(), FoodType.Create("Fish"), new TimeOnly(15, 0), today)
            };
            
            // Setup repository
            _mockFeedingScheduleRepository.Setup(repo => repo.GetByDateAsync(today)).ReturnsAsync(schedules);

            // Act
            var result = await _service.GetTodayFeedingSchedulesAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, schedule => Assert.Equal(today, schedule.ScheduleDate));
        }

        [Fact]
        public async Task GetAnimalFeedingSchedules_ReturnsCorrectSchedules()
        {
            // Arrange
            var animalId = Guid.NewGuid();
            var schedules = new List<FeedingSchedule>
            {
                new FeedingSchedule(animalId, FoodType.Create("Meat"), new TimeOnly(9, 0), DateOnly.FromDateTime(DateTime.Now)),
                new FeedingSchedule(animalId, FoodType.Create("Meat"), new TimeOnly(18, 0), DateOnly.FromDateTime(DateTime.Now.AddDays(1)))
            };
            
            // Setup repository
            _mockFeedingScheduleRepository.Setup(repo => repo.GetByAnimalIdAsync(animalId)).ReturnsAsync(schedules);

            // Act
            var result = await _service.GetAnimalFeedingSchedulesAsync(animalId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, schedule => Assert.Equal(animalId, schedule.AnimalId));
        }
    }
}
