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
    
    public partial class HoSo
    {
        public string TrangThaiHoSo { get; set; }
        public Nullable<System.DateTime> NgayNhanHoSo { get; set; }
        public string NguoiGiaoHoSo { get; set; }
        public string CamKetTuyenDung { get; set; }
        public string QuyetDinhTiepNhanDVC { get; set; }
        public string QDChamDutHopDongDVC { get; set; }
        public string NLDHocTheoChiTieuCTDT { get; set; }
        public string NguoiBanGiaoBangNhapKho { get; set; }
        public string NguoiGiuHoSo { get; set; }
        public string MaNV { get; set; }
        public Nullable<System.DateTime> NgayQuyetDinhTuyenDung { get; set; }
        public Nullable<System.DateTime> NgayDiLam { get; set; }
        public string DonViKyQuyetDinhTiepNhan { get; set; }
        public Nullable<System.DateTime> NgayQuyetDinhChamDut { get; set; }
        public Nullable<System.DateTime> NgayChamDut { get; set; }
        public string DonViKyQuyetDinhChamDut { get; set; }
    
        public virtual NhanVien NhanVien { get; set; }
    }
}
