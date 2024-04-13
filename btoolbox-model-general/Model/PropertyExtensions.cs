using System.Runtime.CompilerServices;

namespace BToolbox.Model
{

    public delegate void PropertyChangedTwoValuesDelegate<TItem, TProperty>(TItem item, TProperty oldValue, TProperty newValue)
        where TItem : INotifyPropertyChanged;
    public delegate void PropertyChangedOneValueDelegate<TItem, TProperty>(TItem item, TProperty newValue)
        where TItem : INotifyPropertyChanged;
    public delegate void PropertyChangedNoValueDelegate<TItem>(TItem item)
        where TItem : INotifyPropertyChanged;

    public class BeforeChangePropertyArgs
    {
        public bool Cancelled { get; private set; } = false;
        public void Cancel() => Cancelled = true;
    }

    public delegate void BeforeChangePropertyDelegate<TProperty>(TProperty oldValue, TProperty newValue, BeforeChangePropertyArgs args);
    public delegate void AfterChangePropertyDelegate<TProperty>(TProperty oldValue, TProperty newValue);
    public delegate void PropertyValidatorDelegate<TProperty>(TProperty value);

    public static class PropertyExtensions
    {

        public static bool setProperty<TItem, TProperty>
               (this TItem item, ref TProperty member, TProperty newValue,
               PropertyChangedTwoValuesDelegate<TItem, TProperty> propertyChanged,
               BeforeChangePropertyDelegate<TProperty> beforeChange = null,
               AfterChangePropertyDelegate<TProperty> afterChange = null,
               PropertyValidatorDelegate<TProperty> validator = null,
               [CallerMemberName] string propertyName = "")
            where TItem : INotifyPropertyChanged
        {
            validator?.Invoke(newValue);
            if (Equals(member, newValue))
                return false;
            TProperty oldValue = member;
            BeforeChangePropertyArgs args = new BeforeChangePropertyArgs();
            beforeChange?.Invoke(oldValue, newValue, args);
            if (args.Cancelled)
                return false;
            member = newValue;
            afterChange?.Invoke(oldValue, newValue);
            propertyChanged?.Invoke(item, oldValue, newValue);
            item.RaisePropertyChanged(propertyName);
            return true;
        }

        public static bool setProperty<TItem, TProperty>
            (this TItem item, ref TProperty member, TProperty newValue, PropertyChangedOneValueDelegate<TItem, TProperty> propertyChanged,
            BeforeChangePropertyDelegate<TProperty> beforeChange = null,
            AfterChangePropertyDelegate<TProperty> afterChange = null,
            PropertyValidatorDelegate<TProperty> validator = null,
            [CallerMemberName] string propertyName = "")
            where TItem : INotifyPropertyChanged
            => item.setProperty(ref member, newValue, (i, ov, nv) => propertyChanged?.Invoke(i, nv), beforeChange, afterChange, validator, propertyName);

        public static bool setProperty<TItem, TProperty>
            (this TItem item, ref TProperty member, TProperty newValue, PropertyChangedNoValueDelegate<TItem> propertyChanged,
            BeforeChangePropertyDelegate<TProperty> beforeChange = null,
            AfterChangePropertyDelegate<TProperty> afterChange = null,
            PropertyValidatorDelegate<TProperty> validator = null,
            [CallerMemberName] string propertyName = "")
            where TItem : INotifyPropertyChanged
            => item.setProperty(ref member, newValue, (i, ov, nv) => propertyChanged?.Invoke(i), beforeChange, afterChange, validator, propertyName);

    }
}