﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Stiem_market.Data
{
    public partial class FriendUsers : INotifyPropertyChanged
    {
        

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
