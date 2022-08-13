namespace B.XmlDeserializer.Helpers;

public static class TypeExtensions
{
    public static string Reference(this Type type) => $"[TYPE:{type}]";
}
