namespace easyvlans.Model.SwitchOperationMethods
{
    internal class NoMappingForPortException : Exception
    {
        public NoMappingForPortException(int localIndex)
            : base($"No mapping defined for port with local index [{localIndex}].")
        { }
    }
}
