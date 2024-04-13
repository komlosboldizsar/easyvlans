using System.Text.RegularExpressions;

namespace BToolbox.XmlDeserializer.Context;

internal class TypenameTranslator
{

    private readonly Dictionary<string, string> typeNames = new();

    public void RegisterTypeName<T>(string name)
        => typeNames.Add(typeof(T).ToString(), name);

    public void RegisterTypeName(Type type, string name)
        => typeNames.Add(type.ToString(), name);

    private static readonly Regex REGEX_TYPEREF = new(@$"\[TYPE:(?<{REGEXP_TYPEREF_GROUP_TYPENAME}>[a-zA-Z0-9_\.]+)\]", RegexOptions.Compiled);
    private const string REGEXP_TYPEREF_GROUP_TYPENAME = "typename";

    public string TranslateTypeNames(string message)
        => REGEX_TYPEREF.Replace(message, match =>
        {
            string typename = match.Groups[REGEXP_TYPEREF_GROUP_TYPENAME].Value;
            if (!typeNames.TryGetValue(typename, out string translatedTypename))
                return typename;
            return translatedTypename;
        });

}
