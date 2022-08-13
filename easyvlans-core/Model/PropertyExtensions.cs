namespace easyvlans.Model
{

    public interface ISetProperty { }

    public delegate void PropertyChangedDelegate<TItem, TProperty>(TItem item, TProperty newValue);

    public static class PropertyExtensions
    {

        public static bool setProperty<TItem, TProperty>(this TItem item, ref TProperty member, TProperty newValue, PropertyChangedDelegate<TItem, TProperty> propertyChangedDelegate)
            where TItem : ISetProperty
        {
            if (Equals(member, newValue))
                return false;
            member = newValue;
            propertyChangedDelegate?.Invoke(item, newValue);
            return true;
        }

    }

}
