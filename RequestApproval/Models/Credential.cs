//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RequestApproval.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Credential
    {
        public Nullable<int> UId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual UserDetail UserDetail { get; set; }
    }
}
