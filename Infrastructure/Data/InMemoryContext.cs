using Zoo.Domain.Entities;

namespace Zoo.Infrastructure.Data
{
    public class InMemoryContext
    {
        public List<Animal> Animals { get; } = new List<Animal>();
        public List<Enclosure> Enclosures { get; } = new List<Enclosure>();
        public List<FeedingSchedule> FeedingSchedules { get; } = new List<FeedingSchedule>();

        // Singleton instance
        private static InMemoryContext _instance;
        private static readonly object _lock = new object();

        private InMemoryContext()
        {
        }

        public static InMemoryContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new InMemoryContext();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
