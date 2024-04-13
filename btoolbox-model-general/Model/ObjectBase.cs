using System.Runtime.CompilerServices;

namespace BToolbox.Model
{
    public class ObjectBase : INotifyPropertyChanged
    {
        public event PropertyChangedDelegate PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(propertyName);
    }
}
