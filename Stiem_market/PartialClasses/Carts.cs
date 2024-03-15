using System;
using System.Linq;

namespace Stiem_market.Data
{
    public partial class Carts
    {
        public int CartCost
        {
            get => App.db.GameInCarts.Where(gic => gic.Cart_id == ID).Sum(item => item.PurchasedCost) ?? 0;
        }
    }
}
