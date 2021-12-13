using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace easyvlans.GUI.Helpers
{
    public static class ControlExtensions
    {
        // @source https://stackoverflow.com/a/10267292
        public static T Clone<T>(this T controlToClone)
            where T : Control
        {
            PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            T instance = Activator.CreateInstance<T>();
            foreach (PropertyInfo propInfo in controlProperties)
                if (propInfo.CanWrite)
                    if (propInfo.Name != "WindowTarget")
                        propInfo.SetValue(instance, propInfo.GetValue(controlToClone, null), null);
            return instance;
        }
    }
}
