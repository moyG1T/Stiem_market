using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Stiem_market.Data
{
    public partial class Games : INotifyPropertyChanged
    {
        private byte[] imageShowcase;
        public byte[] ImageShowcase
        {
            get => imageShowcase ?? ImageBin;
            set
            {
                imageShowcase = value;
                OnPropertyChanged(nameof(ImageShowcase));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
