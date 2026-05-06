using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SchoolJournal.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected Window _current_window;

        public BaseViewModel(Window win)
        {
            _current_window = win;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
