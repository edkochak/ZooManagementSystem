namespace Zoo.Domain.ValueObjects
{
    public class FoodType
    {
        public string Name { get; private set; }
        
        private FoodType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Food name cannot be empty", nameof(name));
                
            Name = name;
        }
        
        public static FoodType Create(string name) => new FoodType(name);
        
        public override string ToString() => Name;
        
        public override bool Equals(object obj)
        {
            if (obj is not FoodType other)
                return false;
                
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }
        
        public override int GetHashCode()
        {
            return Name.ToLowerInvariant().GetHashCode();
        }
    }
}
