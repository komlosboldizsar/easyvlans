using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace easyvlans.GUI.Helpers
{

    // @source https://stackoverflow.com/a/10267292
    public static class Cloning
    {

        public static Control Clone(this Control baseInstance, string[] excludeProperties = null)
        {
            Control newInstance = baseInstance.CloneTypeOnly();
            newInstance.ClonePropertiesFrom(baseInstance, excludeProperties);
            return newInstance;
        }

        public static T CloneT<T>(this T baseInstance, string[] excludeProperties = null)
            where T : Control, new()
        {
            T newInstance = new T();
            newInstance.ClonePropertiesFrom(baseInstance, excludeProperties);
            return newInstance;
        }

        public static Control CloneTypeOnly(this Control baseInstance) => (Control)Activator.CreateInstance(baseInstance.GetType());

        public static void ClonePropertiesFrom(this Control newInstance, Control baseInstance, string[] excludeProperties = null)
        {
            Type baseType = baseInstance.GetType();
            PropertyInfo[] controlProperties = baseType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propInfo in controlProperties)
                if (propInfo.CanWrite)
                    if ((propInfo.Name != "WindowTarget") && (propInfo.Name != "SelectionStart") && (propInfo.Name != "SelectionLength") && (excludeProperties?.Contains(propInfo.Name) != true))
                        propInfo.SetValue(newInstance, propInfo.GetValue(baseInstance, null), null);
        }

        public static readonly string[] EXCLUDE_VISIBILITY = new string[] { nameof(Control.Visible) };

    }

}
