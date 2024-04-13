namespace BToolbox.XmlDeserializer.Exceptions;

internal class DeserializerBuildingException : Exception
{

    public DeserializerBuildingException() { }

    public DeserializerBuildingException(string message)
        : base(message) { }

    public DeserializerBuildingException(string message, Exception innerException)
        : base(message, innerException) { }

}
