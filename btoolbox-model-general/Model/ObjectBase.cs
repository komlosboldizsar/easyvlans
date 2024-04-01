using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BToolbox.Model
{
    public class ObjectBase : INotifyPropertyChanged
    {
        public event PropertyChangedDelegate PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(propertyName);
    }
}
