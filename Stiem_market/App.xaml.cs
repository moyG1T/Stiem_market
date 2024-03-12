using Stiem_market.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Stiem_market
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        //public static StiemEntities db = new StiemEntities();
        public static AbvgdeyozhEntities db = new AbvgdeyozhEntities();
    }
}
