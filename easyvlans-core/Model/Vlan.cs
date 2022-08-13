namespace easyvlans.Model
{
    public class Vlan
    {
        public int ID { get; init; }
        public string Name { get; init; }
        public string Label => $"{ID} - {Name}";
    }
}
