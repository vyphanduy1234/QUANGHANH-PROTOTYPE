﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QUANGHANHABCEntities : DbContext
    {
        public QUANGHANHABCEntities()
            : base("name=QUANGHANHABCEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Acceptance> Acceptances { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Account_Right> Account_Right { get; set; }
        public virtual DbSet<Account_Right_Detail> Account_Right_Detail { get; set; }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<BacLuong> BacLuongs { get; set; }
        public virtual DbSet<BacLuong_ThangLuong_MucLuong> BacLuong_ThangLuong_MucLuong { get; set; }
        public virtual DbSet<BangCap_GiayChungNhan> BangCap_GiayChungNhan { get; set; }
        public virtual DbSet<BaoCaoFile> BaoCaoFiles { get; set; }
        public virtual DbSet<Camera> Cameras { get; set; }
        public virtual DbSet<Camera_Acceptance> Camera_Acceptance { get; set; }
        public virtual DbSet<CameraIncident> CameraIncidents { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarGP> CarGPS { get; set; }
        public virtual DbSet<Category_attribute_value> Category_attribute_value { get; set; }
        public virtual DbSet<ChamDut_NhanVien> ChamDut_NhanVien { get; set; }
        public virtual DbSet<ChiTiet_BangCap_GiayChungNhan> ChiTiet_BangCap_GiayChungNhan { get; set; }
        public virtual DbSet<ChiTiet_NhiemVu_NhanVien> ChiTiet_NhiemVu_NhanVien { get; set; }
        public virtual DbSet<ChungChi> ChungChis { get; set; }
        public virtual DbSet<ChungChi_NhanVien> ChungChi_NhanVien { get; set; }
        public virtual DbSet<ChuyenNganh> ChuyenNganhs { get; set; }
        public virtual DbSet<CongTacAnToan> CongTacAnToans { get; set; }
        public virtual DbSet<CongViec> CongViecs { get; set; }
        public virtual DbSet<CongViec_NhomCongViec> CongViec_NhomCongViec { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<DiemDanh_NangSuatLaoDong> DiemDanh_NangSuatLaoDong { get; set; }
        public virtual DbSet<DienCongViec> DienCongViecs { get; set; }
        public virtual DbSet<DieuDong_NhanVien> DieuDong_NhanVien { get; set; }
        public virtual DbSet<Documentary> Documentaries { get; set; }
        public virtual DbSet<Documentary_big_maintain_details> Documentary_big_maintain_details { get; set; }
        public virtual DbSet<Documentary_camera_repair_details> Documentary_camera_repair_details { get; set; }
        public virtual DbSet<Documentary_Improve_Detail> Documentary_Improve_Detail { get; set; }
        public virtual DbSet<Documentary_liquidation_details> Documentary_liquidation_details { get; set; }
        public virtual DbSet<Documentary_maintain_details> Documentary_maintain_details { get; set; }
        public virtual DbSet<Documentary_moveline_details> Documentary_moveline_details { get; set; }
        public virtual DbSet<Documentary_repair_details> Documentary_repair_details { get; set; }
        public virtual DbSet<Documentary_revoke_details> Documentary_revoke_details { get; set; }
        public virtual DbSet<DocumentaryType> DocumentaryTypes { get; set; }
        public virtual DbSet<Equipment> Equipments { get; set; }
        public virtual DbSet<Equipment_attribute> Equipment_attribute { get; set; }
        public virtual DbSet<Equipment_category> Equipment_category { get; set; }
        public virtual DbSet<Equipment_category_attribute> Equipment_category_attribute { get; set; }
        public virtual DbSet<Equipment_Inspection> Equipment_Inspection { get; set; }
        public virtual DbSet<Equipment_Insurance> Equipment_Insurance { get; set; }
        public virtual DbSet<Equipment_SCTX> Equipment_SCTX { get; set; }
        public virtual DbSet<Equipment_SCTX_Detail> Equipment_SCTX_Detail { get; set; }
        public virtual DbSet<FakeAPI> FakeAPIs { get; set; }
        public virtual DbSet<FileBaoCao> FileBaoCaos { get; set; }
        public virtual DbSet<Fuel_activities_consumption> Fuel_activities_consumption { get; set; }
        public virtual DbSet<GiayTo> GiayToes { get; set; }
        public virtual DbSet<Header_DiemDanh_NangSuat_LaoDong> Header_DiemDanh_NangSuat_LaoDong { get; set; }
        public virtual DbSet<header_KeHoach_TieuChi_TheoNam> header_KeHoach_TieuChi_TheoNam { get; set; }
        public virtual DbSet<header_KeHoach_TieuChi_TheoNgay> header_KeHoach_TieuChi_TheoNgay { get; set; }
        public virtual DbSet<header_KeHoachTungThang> header_KeHoachTungThang { get; set; }
        public virtual DbSet<header_ThucHienTheoNgay> header_ThucHienTheoNgay { get; set; }
        public virtual DbSet<HoSo> HoSoes { get; set; }
        public virtual DbSet<Important_Documentary> Important_Documentary { get; set; }
        public virtual DbSet<Incident> Incidents { get; set; }
        public virtual DbSet<KeHoach_TieuChi_TheoNam> KeHoach_TieuChi_TheoNam { get; set; }
        public virtual DbSet<KeHoach_TieuChi_TheoNgay> KeHoach_TieuChi_TheoNgay { get; set; }
        public virtual DbSet<KeHoach_TieuChi_TheoThang> KeHoach_TieuChi_TheoThang { get; set; }
        public virtual DbSet<LichSuBoSungSYLL> LichSuBoSungSYLLs { get; set; }
        public virtual DbSet<Maintain_Car> Maintain_Car { get; set; }
        public virtual DbSet<Maintain_Car_Detail> Maintain_Car_Detail { get; set; }
        public virtual DbSet<MealRegistration> MealRegistrations { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Nganh> Nganhs { get; set; }
        public virtual DbSet<NguoiUyQuyenLayHoSo_BaoHiem> NguoiUyQuyenLayHoSo_BaoHiem { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<NhiemVu> NhiemVus { get; set; }
        public virtual DbSet<NhomCongViec> NhomCongViecs { get; set; }
        public virtual DbSet<NhomTieuChi> NhomTieuChis { get; set; }
        public virtual DbSet<PhongBan_TieuChi> PhongBan_TieuChi { get; set; }
        public virtual DbSet<QuanHeGiaDinh> QuanHeGiaDinhs { get; set; }
        public virtual DbSet<QuaTrinhCongTac> QuaTrinhCongTacs { get; set; }
        public virtual DbSet<QuyetDinh> QuyetDinhs { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Supply> Supplies { get; set; }
        public virtual DbSet<Supply_DiKem> Supply_DiKem { get; set; }
        public virtual DbSet<Supply_Documentary_Camera> Supply_Documentary_Camera { get; set; }
        public virtual DbSet<Supply_Documentary_Equipment> Supply_Documentary_Equipment { get; set; }
        public virtual DbSet<Supply_DuPhong> Supply_DuPhong { get; set; }
        public virtual DbSet<Supply_tieuhao> Supply_tieuhao { get; set; }
        public virtual DbSet<SupplyPlan> SupplyPlans { get; set; }
        public virtual DbSet<TaiNan> TaiNans { get; set; }
        public virtual DbSet<ThangLuong> ThangLuongs { get; set; }
        public virtual DbSet<ThucHien_TieuChi_TheoNgay> ThucHien_TieuChi_TheoNgay { get; set; }
        public virtual DbSet<TieuChi> TieuChis { get; set; }
        public virtual DbSet<TrangThai> TrangThais { get; set; }
        public virtual DbSet<TrinhDo> TrinhDoes { get; set; }
        public virtual DbSet<Truong> Truongs { get; set; }
        public virtual DbSet<TuyenDung_NhanVien> TuyenDung_NhanVien { get; set; }
        public virtual DbSet<User_Action_Log> User_Action_Log { get; set; }
        public virtual DbSet<Disk> Disks { get; set; }
    }
}
