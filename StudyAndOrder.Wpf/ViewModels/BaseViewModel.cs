//Fortæller WPF når en værdi ændrer sig
// Sender besked til UI om at opdatere sig
// Sætter en værdi og sender besked automatisk

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudyAndOrder.Wpf.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}