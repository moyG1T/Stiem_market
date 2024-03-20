namespace Stiem_market.Data
{
    public partial class GameInCarts
    {
        private bool inLibrary = false;
        public bool InLibrary
        {
            get
            {
                return inLibrary;
            }
            set
            {
                inLibrary = value;
            }
        }
    }
}
