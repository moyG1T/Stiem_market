using Stiem_market.Data;
using System.Windows;

namespace Stiem_market
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static StiemEntities db = new StiemEntities();
        //public static AbvgdeyozhEntities db = new AbvgdeyozhEntities();
    }
}
