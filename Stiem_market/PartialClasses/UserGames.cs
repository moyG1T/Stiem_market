using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Stiem_market.Data
{
    public partial class UserGames : INotifyPropertyChanged
    {
        //private bool isInCart;
        //public bool IsInCart
        //{
        //    get => isInCart;
        //    set
        //    {
        //        isInCart = value;
        //        OnPropertyChanged(nameof(IsInCart));
        //    }
        //}


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
