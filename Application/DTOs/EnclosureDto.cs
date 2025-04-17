namespace Zoo.Application.DTOs
{
    public class EnclosureDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentAnimalsCount { get; set; }
        public List<Guid> AnimalIds { get; set; } = new List<Guid>();
    }
}
