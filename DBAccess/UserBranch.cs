//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserBranch
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int branch_id { get; set; }
    
        public virtual Branches Branches { get; set; }
        public virtual Users Users { get; set; }
    }
}