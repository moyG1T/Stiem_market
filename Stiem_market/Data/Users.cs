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
    
    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            this.Carts = new HashSet<Carts>();
            this.Carts1 = new HashSet<Carts>();
            this.FriendUsers = new HashSet<FriendUsers>();
            this.FriendUsers1 = new HashSet<FriendUsers>();
        }
    
        //public int ID { get; set; }
        //public string Nickname { get; set; }
        //public byte[] Avatar { get; set; }
        //public string Email { get; set; }
        //public string Password { get; set; }
        //public Nullable<bool> IsDev { get; set; }
        //public Nullable<bool> IsDeleted { get; set; }
        //public string Description { get; set; }
        //public int Balance { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Carts> Carts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Carts> Carts1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FriendUsers> FriendUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FriendUsers> FriendUsers1 { get; set; }
    }
}
