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
    
    public partial class FakeAPI
    {
        public string MaNV { get; set; }
        public System.DateTime NgayDiemDanh { get; set; }
        public int CaDiemDanh { get; set; }
        public Nullable<System.DateTime> GioDiemDanh { get; set; }
    
        public virtual NhanVien NhanVien { get; set; }
    }
}
