using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Stiem_market.Data
{
    public partial class Users
    {
        private int id;
        private string nickname;
        private string email;
        private string password;
        private bool isDev;
        private bool isDeleted;
        private byte[] avatar;
        private string description;
        private int balance;

        public int ID
        {
            get => id;
            set { id = value; OnPropertyChanged("ID"); }
        }
        public string Nickname
        {
            get => nickname;
            set { nickname = value; OnPropertyChanged("Nickname"); }
        }
        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged("Email"); }
        }
        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged("Password"); }
        }
        public bool IsDev
        {
            get => isDev;
            set { isDev = value; OnPropertyChanged("IsDev"); }
        }
        public bool IsDeleted
        {
            get => isDeleted;
            set { isDeleted = value; OnPropertyChanged("IsDeleted"); }
        }
        public byte[] Avatar
        {

            get => avatar ?? File.ReadAllBytes(".\\Resources\\profileIcon.png");
            set
            {
                avatar = value;
                OnPropertyChanged("Avatar");
            }
        }
        public string Description
        {
            get => description == "" ? "Нет описания" : description;
            set { description = value; OnPropertyChanged("Description"); }
        }
        public int Balance
        {
            get => balance;
            set { balance = value; OnPropertyChanged(nameof(Balance)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
