//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Stiem_market.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class GameShowcase
    {
        public int ID { get; set; }
        public Nullable<int> Game_id { get; set; }
        public byte[] Image { get; set; }
    
        public virtual Games Games { get; set; }
    }
}
