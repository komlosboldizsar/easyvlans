using System.Reflection;

namespace BToolbox.Model
{
    public class MultilevelPropertyChangeObserver
    {

        public MultilevelPropertyChangeObserver(INotifyPropertyChanged baseItem, string[] propertyNames, object tag = null)
        {
            Tag = tag;
            INotifyPropertyChanged item = baseItem;
            string fullPropertyName = "";
            Level previousLevel = null;
            foreach (string propertyName in propertyNames)
            {
                fullPropertyName += "." + propertyName;
                if (fullPropertyName[0] == '.')
                    fullPropertyName = fullPropertyName.Substring(1);
                Level thisLevel = new Level(this, propertyName, fullPropertyName);
                thisLevel.ObservedItem = item;
                previousLevel?.LinkNextLevel(thisLevel);
                previousLevel = thisLevel;
                item = getPropertyValue(item, propertyName);
            }
        }

        public delegate void MultilevelPropertyChangedDelegate(string fullPropertyName, MultilevelPropertyChangeObserver observer);
        public event MultilevelPropertyChangedDelegate MultilevelPropertyChanged;

        public object Tag { get; private set; }

        private void propertyChangedOnLevel(Level level)
        {
            MultilevelPropertyChanged(level.FullPropertyName, this);
        }

        private static INotifyPropertyChanged getPropertyValue(INotifyPropertyChanged owner, string propertyName)
        {
            if (owner == null)
                return null;
            Type ownerType = owner.GetType();
            PropertyInfo ownerProperty = ownerType.GetProperty(propertyName, MEMBER_LOOKUP_BINDING_FLAGS);
            object propertyValue = ownerProperty?.GetValue(owner);
            return (propertyValue as INotifyPropertyChanged);
        }

        private const BindingFlags MEMBER_LOOKUP_BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        List<Level> levels = new List<Level>();

        private class Level
        {

            private MultilevelPropertyChangeObserver parent;

            public Level(MultilevelPropertyChangeObserver parent, string propertyName, string fullPropertyName)
            {
                this.parent = parent;
                PropertyName = propertyName;
                FullPropertyName = fullPropertyName;
            }

            public string PropertyName { get; private set; }
            public string FullPropertyName { get; private set; }

            private void propertyChangedHandler(string propertyName)
            {
                if (!string.Equals(propertyName, PropertyName))
                    return;
                INotifyPropertyChanged subItem = getPropertyValue(observedItem, propertyName);
                if (NextLevel != null)
                    NextLevel.ObservedItem = subItem;
                parent.propertyChangedOnLevel(this);
            }

            private INotifyPropertyChanged observedItem;
            public INotifyPropertyChanged ObservedItem
            {
                get => observedItem;
                set
                {
                    if (observedItem == value)
                        return;
                    if (observedItem != null)
                    {
                        observedItem.PropertyChanged -= propertyChangedHandler;
                    }
                    observedItem = value;
                    if (observedItem != null)
                    {
                        observedItem.PropertyChanged += propertyChangedHandler;
                        if (NextLevel != null)
                        {
                            INotifyPropertyChanged subItem = getPropertyValue(observedItem, PropertyName);
                            NextLevel.ObservedItem = subItem;
                        }
                    }
                }
            }

            public Level NextLevel { get; private set; }

            public void LinkNextLevel(Level nextLevel)
            {
                NextLevel = nextLevel;
            }

        }

    }
}
