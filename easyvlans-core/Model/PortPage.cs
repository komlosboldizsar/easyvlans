namespace easyvlans.Model
{
    public class PortPage : List<Port>
    {
        public string Title { get; init; }
        public bool IsDefault { get; init; }
    }
}
