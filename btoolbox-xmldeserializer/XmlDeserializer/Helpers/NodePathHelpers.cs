using System.Xml;

namespace BToolbox.XmlDeserializer.Helpers;

public static class NodePathHelpers
{

    public static string GetPath(this XmlNode node)
    {
        List<string> strings = new();
        XmlDocument ownerDocument = node.OwnerDocument;
        XmlNode rootElement = ownerDocument.DocumentElement;
        while (node != ownerDocument)
        {
            string nodeName = node.LocalName;
            if (node is XmlAttribute attribute)
            {
                strings.Add($"#{nodeName}");
                node = attribute.OwnerElement;
            }
            else // node is XmlElement
            {
                string indexString = node == rootElement ? "ROOT" : (node.GetIndex() + 1).ToString();
                strings.Add($"<{indexString}:{nodeName}>");
                node = node.ParentNode;
            }
        }
        strings.Reverse();
        return string.Join('/', strings);
    }

    public static int GetIndex(this XmlNode node)
    {
        XmlNode parentNode = node.ParentNode;
        if (parentNode == null)
            return -1;
        return parentNode.ChildNodes.GetElementIndex(node);
    }

}
