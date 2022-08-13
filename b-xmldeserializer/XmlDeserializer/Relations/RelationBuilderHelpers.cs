using B.XmlDeserializer.Context;
using B.XmlDeserializer.Exceptions;
using System.Xml;

namespace B.XmlDeserializer.Relations;

public static class RelationBuilderHelpers
{

    public static void HandleExceptions<TElement, TEnvironment>(Action<XmlNode, TElement, TEnvironment, DeserializationContext> builder, XmlNode elementNode, TElement element, TEnvironment environment, DeserializationContext context)
    {
        try
        {
            builder.Invoke(elementNode, element, environment, context);
        }
        catch (DeserializationException ex)
        {
            context.ReportInvalidRelation(ex.XmlNode, element, ex.Message);
        }
    }

}
