namespace Zoo.Domain.ValueObjects
{
    public class EnclosureType
    {
        public string Name { get; private set; }
        
        private EnclosureType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Enclosure type name cannot be empty", nameof(name));
                
            Name = name;
        }
        
        public static EnclosureType Create(string name) => new EnclosureType(name);
        
        public static EnclosureType Predator => new EnclosureType("Predator");
        public static EnclosureType Herbivore => new EnclosureType("Herbivore");
        public static EnclosureType Aviary => new EnclosureType("Aviary");
        public static EnclosureType Aquarium => new EnclosureType("Aquarium");
        
        public override string ToString() => Name;
        
        public override bool Equals(object obj)
        {
            if (obj is not EnclosureType other)
                return false;
                
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }
        
        public override int GetHashCode()
        {
            return Name.ToLowerInvariant().GetHashCode();
        }
    }
}
