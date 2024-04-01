using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BToolbox.Model
{
    public interface IObservableList<T> : IObservableCollection<T>, IList<T>
    { }
}
