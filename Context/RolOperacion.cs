//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WEBCAM.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class RolOperacion
    {
        public System.Guid Id { get; set; }
        public string IdRol { get; set; }
        public System.Guid IdOperacion { get; set; }
        public List<Operaciones> ListadoOperaciones { get; set; }
        public List<Modulo> ListadoModulos { get; set; }
        public virtual AspNetRoles AspNetRoles { get; set; }
        public virtual Operaciones Operaciones { get; set; }
    }
}
