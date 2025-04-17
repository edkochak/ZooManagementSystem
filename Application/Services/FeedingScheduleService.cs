using Zoo.Application.DTOs;
using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;
using Zoo.Domain.Interfaces;
using Zoo.Domain.ValueObjects;

namespace Zoo.Application.Services
{
    public class FeedingScheduleService : IFeedingScheduleService
    {
        private readonly IFeedingScheduleRepository _feedingScheduleRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly FeedingOrganizationService _feedingOrganizationService;

        public FeedingScheduleService(
            IFeedingScheduleRepository feedingScheduleRepository,
            IAnimalRepository animalRepository,
            FeedingOrganizationService feedingOrganizationService)
        {
            _feedingScheduleRepository = feedingScheduleRepository;
            _animalRepository = animalRepository;
            _feedingOrganizationService = feedingOrganizationService;
        }

        public async Task<IEnumerable<FeedingScheduleDto>> GetAllSchedulesAsync()
        {
            var schedules = await _feedingScheduleRepository.GetAllAsync();
            return schedules.Select(MapToDto);
        }

        public async Task<FeedingScheduleDto> GetScheduleByIdAsync(Guid id)
        {
            var schedule = await _feedingScheduleRepository.GetByIdAsync(id);
            return schedule != null ? MapToDto(schedule) : null;
        }

        public async Task<IEnumerable<FeedingScheduleDto>> GetSchedulesByAnimalIdAsync(Guid animalId)
        {
            var schedules = await _feedingScheduleRepository.GetByAnimalIdAsync(animalId);
            return schedules.Select(MapToDto);
        }

        public async Task<IEnumerable<FeedingScheduleDto>> GetSchedulesByDateAsync(DateOnly date)
        {
            var schedules = await _feedingScheduleRepository.GetByDateAsync(date);
            return schedules.Select(MapToDto);
        }

        public async Task<Guid> AddScheduleAsync(FeedingScheduleDto scheduleDto)
        {
            // Use the specialized service to handle scheduling
            return await _feedingOrganizationService.ScheduleFeedingAsync(
                scheduleDto.AnimalId,
                scheduleDto.FoodType,
                scheduleDto.FeedingTime,
                scheduleDto.ScheduleDate
            );
        }

        public async Task UpdateScheduleAsync(FeedingScheduleDto scheduleDto)
        {
            var schedule = await _feedingScheduleRepository.GetByIdAsync(scheduleDto.Id);
            if (schedule == null)
                throw new ArgumentException($"Feeding schedule with ID {scheduleDto.Id} not found");
                
            // Update food type if different
            if (schedule.FoodType.Name != scheduleDto.FoodType)
            {
                schedule.ChangeFoodType(FoodType.Create(scheduleDto.FoodType));
            }
            
            // Update feeding time if different
            if (schedule.FeedingTime != scheduleDto.FeedingTime)
            {
                schedule.ChangeTime(scheduleDto.FeedingTime);
            }
            
            // Update completed status if different
            if (schedule.IsCompleted != scheduleDto.IsCompleted)
            {
                if (scheduleDto.IsCompleted)
                    schedule.MarkAsCompleted();
                else
                    schedule.ResetCompletion();
            }
            
            await _feedingScheduleRepository.UpdateAsync(schedule);
        }

        public async Task DeleteScheduleAsync(Guid id)
        {
            await _feedingScheduleRepository.DeleteAsync(id);
        }

        public async Task MarkScheduleAsCompletedAsync(Guid id)
        {
            await _feedingOrganizationService.MarkFeedingCompletedAsync(id);
        }

        private FeedingScheduleDto MapToDto(FeedingSchedule schedule)
        {
            return new FeedingScheduleDto
            {
                Id = schedule.Id,
                AnimalId = schedule.AnimalId,
                FoodType = schedule.FoodType.Name,
                FeedingTime = schedule.FeedingTime,
                ScheduleDate = schedule.ScheduleDate,
                IsCompleted = schedule.IsCompleted
            };
        }
    }
}
