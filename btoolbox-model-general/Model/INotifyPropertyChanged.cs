using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BToolbox.Model
{
    public delegate void PropertyChangedDelegate(string propertyName);
    public interface INotifyPropertyChanged
    {
        event PropertyChangedDelegate PropertyChanged;
        void RaisePropertyChanged(string propertyName);
    }
}
