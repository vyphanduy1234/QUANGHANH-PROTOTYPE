//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QUANGHANH2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SupplyPlan
    {
        public int id { get; set; }
        public string supplyid { get; set; }
        public string departmentid { get; set; }
        public string equipmentid { get; set; }
        public System.DateTime date { get; set; }
        public Nullable<double> dinh_muc { get; set; }
        public int quantity_plan { get; set; }
        public int quantity { get; set; }
        public Nullable<int> status { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Department Department1 { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual Supply Supply { get; set; }
        public virtual Supply Supply1 { get; set; }
    }
}
