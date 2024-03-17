using System;
using System.Linq;

namespace Stiem_market.Data
{
    public partial class Carts
    {
        private string cartCost;
        public string CartCost
        { 
            get
            {
                if (Purchaser_id != Properties.Settings.Default.LoggedUserID)
                {
                    cartCost = "Подарок";
                }
                else
                {
                    cartCost = App.db.GameInCarts.Where(gic => gic.Cart_id == ID).Sum(item => item.PurchasedCost ?? 0).ToString($"{0}₽");
                }

                return cartCost;
            }
        }
    }
}
