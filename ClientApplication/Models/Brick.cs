using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClientApplication.Models
{
    public class Brick : INotifyPropertyChanged
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; } = 64;
        public int Height { get; } = 40;
        
        private bool isVisible = true;
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
