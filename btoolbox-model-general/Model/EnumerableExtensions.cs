namespace BToolbox.Model
{
    public static class EnumerableExtensions
    {

        public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T element in enumerable)
                action(element);
        }

        public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            int i = 0;
            foreach (T element in enumerable)
                action(element, i++);
        }

        public static IEnumerable<T> Findall<T>(this IEnumerable<T> enumerable, Predicate<T> match)
        {
            List<T> results = new();
            foreach (T element in enumerable)
                if (match?.Invoke(element) == true)
                    results.Add(element);
            return results;
        }

        public static int GetIndexOf<T>(this IEnumerable<T> enumerable, T element, IEqualityComparer<T> comparer = null)
        {
            comparer ??= EqualityComparer<T>.Default;
            int index = 0;
            foreach (T _element in enumerable)
            {
                if (comparer.Equals(_element, element))
                    return index;
                index++;
            }
            return -1;
        }

    }
}
