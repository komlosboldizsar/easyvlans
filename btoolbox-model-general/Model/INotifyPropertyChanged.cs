namespace BToolbox.Model
{
    public delegate void PropertyChangedDelegate(string propertyName);
    public interface INotifyPropertyChanged
    {
        event PropertyChangedDelegate PropertyChanged;
        void RaisePropertyChanged(string propertyName);
    }
}
