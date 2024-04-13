using System.Collections;

namespace BToolbox.XmlDeserializer.Helpers;

public static class EnumerableHelpers
{

    public static int GetElementIndex(this IEnumerable enumerable, object element)
    {
        int i = 0;
        foreach (object obj in enumerable)
        {
            if (obj == element)
                return i;
            i++;
        }
        return -1;
    }

    public static int GetElementIndex<T>(this IEnumerable<T> enumerable, T element)
        => enumerable.GetElementIndex(element);

}
