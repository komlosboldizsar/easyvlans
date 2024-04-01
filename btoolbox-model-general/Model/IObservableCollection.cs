using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BToolbox.Model
{
    public interface IObservableCollection<T> : IObservableEnumerable<T>, ICollection<T>
    { }
}
