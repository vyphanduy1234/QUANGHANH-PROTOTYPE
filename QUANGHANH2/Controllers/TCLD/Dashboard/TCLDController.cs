﻿using QUANGHANH2.Models;
using QUANGHANH2.SupportClass;
using QUANGHANHCORE.Controllers.PX.PXKT;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace QUANGHANHCORE.Controllers.TCLD
{
    /// <summary>
    /// Defines the <see cref="TCLDController" />
    /// </summary>
    public class TCLDController : Controller
    {
        // GET: /<controller>/
        /// <summary>
        /// The Dashboard
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [Auther(RightID = "002")]
        [Route("phong-tcld")]
        public ActionResult Dashboard()
        {
            int soLuotHuyDong = 0;
            int vuTaiNan = 0;
            int nghiVLD = 0;
            int hetHanChungChi = 0;
            int tren82 = 0;
            int duoi82 = 0;
            List<NghiVLD> listNghiVLD = new List<NghiVLD>();
            List<NhanLuc> listNhanLuc = new List<NhanLuc>();
            SanLuong sanluong = new SanLuong();
            int temp = 0;
            using (QUANGHANHABCEntities db = new QUANGHANHABCEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                ////////////////////////////GET so luot huy dong////////////////////////////////
                string sql = "select (case when count(MaQuyetDinh)  is null then 0 else count(MaQuyetDinh) end ) as SoLuotHuyDong from quyetdinh\n" +
                "where maquyetdinh in\n" +
                "(SELECT  distinct dd.MaQuyetDinh FROM DIEUDONG_NHANVIEN dd,QuyetDinh qd where dd.MaQuyetDinh=qd.MaQuyetDinh and qd.SoQuyetDinh<>'' )\n" +
                "AND NgayQuyetDinh = (SELECT CONVERT(VARCHAR(10), getdate() - 1, 101))";

                try
                {
                    temp = db.Database.SqlQuery<int>(sql).ToList<int>()[0];
                    soLuotHuyDong = temp != null ? temp : 0;
                }
                catch (Exception e)
                {

                }
                ////////////////////////////GET SO LUONG TAI NAN///////////////////////////////////////////
                sql = "select (case when Count(tn.MaNV) is null then 0 else Count(tn.MaNV) end )  from \n" +
                      "(select MaNV, Ngay from TaiNan where\n" +
                      "Ngay = (SELECT CONVERT(VARCHAR(10), getdate() - 1, 101))) as tn";

                try
                {
                    temp = db.Database.SqlQuery<int>(sql).ToList<int>()[0];
                    vuTaiNan = temp != null ? temp : 0;
                }
                catch (Exception e)
                {

                }
                ///////////////////////////////GET SO LUONG HET HAN CC///////////////////////////////////////////
                sql = "select (case when sum(th.st)  is null then 0 else sum(th.st) end ) \n" +
                          "from(select cn.MaNV, cn.NgayCap, cc.ThoiHan, (case\n" +
                          "when DATEADD(MONTH, cc.ThoiHan, cn.NgayCap) <= GETDATE()\n" +
                          "then 1 else 0 end) as st\n" +
                          "from ChungChi_NhanVien cn join ChungChi cc on cn.MaChungChi = cc.MaChungChi) as th";


                try
                {
                    temp = db.Database.SqlQuery<int>(sql).ToList<int>()[0];
                    hetHanChungChi = temp != null ? temp : 0;
                }
                catch (Exception e)
                {

                }
                ////////////////////////////GET SO LUONG NGHI VLD///////////////////////////////////////////
                sql = @"select case when Count(b.MaNV) is null then 0 else count(b.MaNV) end 'SoLuongNhanVien' from Header_DiemDanh_NangSuat_LaoDong a join DiemDanh_NangSuatLaoDong b on a.HeaderID = b.HeaderID
                        where a.NgayDiemDanh = (SELECT CONVERT(VARCHAR(10), getdate() - 1, 101)) and b.LyDoVangMat = N'Vô lý do'
                        group by a.NgayDiemDanh, b.LyDoVangMat";

                try
                {
                    temp = db.Database.SqlQuery<int>(sql).ToList<int>()[0];
                    nghiVLD = temp != null ? temp : 0;
                }
                catch (Exception e)
                {

                }
                /////////////////////////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////GET TI LE HUY DONG////////////////////////////////////////
                String currentDate = DateTime.Now.Date.AddDays(-1).ToString("yyyy/MM/dd");
                try
                {
                    sql = @"select a.department_id, a.QL, (a.KT + a.CD) as Tong, a.KT, a.CD, 0 as 'HSTT', 
                                a.dilam, (a.vld + a.om + a.khac + a.phep) as vang, 
                                a.vld,a.om,a.phep,a.khac, 
                                (case when (a.KT+ a.CD) = 0 then 0 else round(Convert(float,(a.KT + a.CD - a.tong_nghidai)-(a.vld + a.om + a.khac + a.phep))/(a.KT + a.CD - a.tong_nghidai)*100,1) end) as tile, 
                                b.than, b.metlo, b.xen,b.diemluong,a.tong_nghidai,a.nghidai_om,a.nghidai_thld,a.nghidai_vld 
                                from 
                                (select a.department_id, a.QL, a.KT, a.CD, 
                                sum(case when d.DiLam = 1 and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as dilam, 
                                sum(case when d.DiLam = 0  and d.LyDoVangMat like N'Vô lý do' and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'vld', 
                                sum(case when d.DiLam = 0  and d.LyDoVangMat like N'Ốm' and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'om', 
                                sum(case when d.DiLam = 0  and d.LyDoVangMat like N'Nghỉ phép' and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'phep', 
                                sum(case when d.DiLam = 0  and d.LyDoVangMat like N'Khác' and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'khac', 
                                SUM(case when d.LyDoVangMat in (N'Tai nạn lao động',N'Ốm dài',N'Thai sản',N'Tạm hoãn lao động',N'Vô lý do dài') and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'tong_nghidai', 
                                SUM(case when d.LyDoVangMat in (N'Vô lý do dài') and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'nghidai_vld', 
                                SUM(case when d.LyDoVangMat in (N'Tạm hoãn lao động') and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'nghidai_thld', 
                                SUM(case when d.LyDoVangMat in (N'Ốm dài') and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'nghidai_om' 
                                 from(select a.department_id, 
                                sum(case when ncv.MaNhomCongViec = 10 then  1 else 0 end) as QL, 
                                sum(case when ncv.MaNhomCongViec = 6 then  1 else 0 end) as KT, 
                                sum(case when ncv.MaNhomCongViec = 7 then  1 else 0 end) as CD 
                                from Department a left outer join NhanVien n on n.MaPhongBan = a.department_id 
                                join CongViec_NhomCongViec cn on n.MaCongViec = cn.MaCongViec 
                                join NhomCongViec ncv on cn.MaNhomCongViec = ncv.MaNhomCongViec 
                                where a.department_type like N'%chính%' and a.department_id != 'PXST' and a.department_id != 'PXLT' 
                                group by a.department_id) 
                                 as a left outer join Header_DiemDanh_NangSuat_LaoDong h 
                                on a.department_id = h.MaPhongBan left outer join DiemDanh_NangSuatLaoDong d 
                                on h.HeaderID = d.HeaderID 
                                group by a.department_id, a.QL, a.KT, a.CD) as a inner join 
                                ( select a.department_id, 
                                sum(case when h.ThanThucHien is not null and h.NgayDiemDanh = @NgayDiemDanh then h.ThanThucHien else 0 end) as 'than', 
                                sum(case when h.MetLoThucHien is not null and h.NgayDiemDanh = @NgayDiemDanh then h.MetLoThucHien else 0 end) as 'metlo', 
                                sum(case when h.XenThucHien is not null and h.NgayDiemDanh = @NgayDiemDanh then h.XenThucHien else 0 end) as 'xen', 
                                sum(case when h.TotalEffort is not null and h.NgayDiemDanh = @NgayDiemDanh then h.TotalEffort else 0 end) as 'diemluong' 
                                from Department a left outer join Header_DiemDanh_NangSuat_LaoDong h 
                                on a.department_id = h.MaPhongBan 
                                group by a.department_id 
                                ) as b on a.department_id = b.department_id";
                    List<BaoCaoNgayDB> listTLHD = db.Database.SqlQuery<BaoCaoNgayDB>(sql, new SqlParameter("NgayDiemDanh", currentDate)).ToList();
                    for (int i = 0; i < listTLHD.Count; i++)
                    {
                        if (listTLHD[i].tile > 82)
                        {
                            tren82++;
                        }
                        else
                        {
                            duoi82++;
                        }
                    }
                }
                catch (Exception e)
                {

                }
                //////////////////////////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////GET NV NGHI VLD////////////////////////////////////////
                sql = "select n.MaNV, n.Ten as HoTen,Department.department_name as TenDonVi\n" +
                "from Department, Header_DiemDanh_NangSuat_LaoDong hd inner join DiemDanh_NangSuatLaoDong d\n" +
                "on hd.HeaderID = d.HeaderID and hd.NgayDiemDanh = (SELECT CONVERT(VARCHAR(10), getdate() - 1, 101)) and d.LyDoVangMat like N'Vô lý do' inner join NhanVien n\n" +
                "on d.MaNV = n.MaNV\n" +
                "where Department.department_id = hd.MaPhongBan";
                try
                {
                    listNghiVLD = db.Database.SqlQuery<NghiVLD>(sql).ToList<NghiVLD>();
                }
                catch (Exception e)
                {

                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////

                ////////////////////////////////////////GET DATA NHAN LUC////////////////////////////////////////////////
                sql = "select tb1.department_id as MaDonVi,\n" +
                        "(case when tb2.soluong is null then 0 else tb2.soluong end) as SoLuong\n" +
                        "from\n" +
                        "(select * from Department where department_id in\n" +
                        "('PXKT1', 'PXKT2', 'PXKT3', 'PXKT4', 'PXKT5', 'PXKT6', 'PXKT7', 'PXKT8', 'PXKT9', 'PXKT10', 'PXKT11',\n" +
                        "'PXDL3', 'PXDL5', 'PXDL7', 'PXDL8', 'PXVT1', 'PXVT2')) tb1\n" +
                        "left join\n" +
                        "(select hd.MaPhongBan, count(d.MaNV) as soluong from Header_DiemDanh_NangSuat_LaoDong hd inner\n" +
                         "                                               join DiemDanh_NangSuatLaoDong d\n" +
                        "on hd.HeaderID = d.HeaderID\n" +
                        "where hd.NgayDiemDanh = (SELECT CONVERT(VARCHAR(10), getdate() - 1, 101)) and DiLam = 1\n" +
                        "group by hd.MaPhongBan) tb2\n" +
                        "on tb1.department_id = tb2.MaPhongBan\n" +
                        "group by tb1.department_id,tb2.soluong";
                try
                {
                    listNhanLuc = db.Database.SqlQuery<NhanLuc>(sql).ToList<NhanLuc>();
                }
                catch (Exception e)
                {

                }

                ///////////////////////////////////////GET DATA SAN LUONG///////////////////////////////////////////////
                sql = "select (select (case when sum(tc_kh.SanLuongKeHoach) is null then 0 else sum(tc_kh.SanLuongKeHoach) end) from (select tc.MaTieuChi, tc.DonViDo, kh.SanLuongKeHoach, kh.ThangKeHoach, kh.NamKeHoach from KeHoach_TieuChi kh join TieuChi tc on kh.MaTieuChi = tc.MaTieuChi) as tc_kh where tc_kh.MaTieuChi in (1,2,3,4) and  tc_kh.ThangKeHoach = (SELECT MONTH(CONVERT(VARCHAR(10), getdate() - 1, 101))) and tc_kh.NamKeHoach = (SELECT YEAR(CONVERT(VARCHAR(10), getdate() - 1, 101)))) 'SLKH', \n" +
                "(select (case when sum(tc_kh.SanLuongKeHoach) is null then 0 else sum(tc_kh.SanLuongKeHoach) end) from (select tc.MaTieuChi, tc.DonViDo, kh.SanLuongKeHoach, kh.ThangKeHoach, kh.NamKeHoach from KeHoach_TieuChi kh join TieuChi tc on kh.MaTieuChi = tc.MaTieuChi) as tc_kh where tc_kh.MaTieuChi in (7,8) and  tc_kh.ThangKeHoach = (SELECT MONTH(CONVERT(VARCHAR(10), getdate() - 1, 101))) and tc_kh.NamKeHoach = (SELECT YEAR(CONVERT(VARCHAR(10), getdate() - 1, 101)))) 'MLKH',\n" +
                "(select (case when sum(tc_th.SanLuongThucHien) is null then 0 else sum(tc_th.SanLuongThucHien) end) from (select tc.MaTieuChi, tc.DonViDo, th.SanLuongThucHien, th.NgayThucHien from ThucHien_TieuChi th join TieuChi tc on th.MaTieuChi = tc.MaTieuChi) as tc_th where tc_th.MaTieuChi in (1,2,3,4) and  MONTH(tc_th.NgayThucHien) = (SELECT MONTH(CONVERT(VARCHAR(10), getdate() - 1, 101))) and YEAR(tc_th.NgayThucHien) = (SELECT YEAR(CONVERT(VARCHAR(10), getdate() - 1, 101)))) 'LKSL',\n" +
                "(select (case when sum(tc_th.SanLuongThucHien) is null then 0 else sum(tc_th.SanLuongThucHien) end) from (select tc.MaTieuChi, tc.DonViDo, th.SanLuongThucHien, th.NgayThucHien from ThucHien_TieuChi th join TieuChi tc on th.MaTieuChi = tc.MaTieuChi) as tc_th where tc_th.MaTieuChi in (7,8) and  MONTH(tc_th.NgayThucHien) = (SELECT MONTH(CONVERT(VARCHAR(10), getdate() - 1, 101))) and YEAR(tc_th.NgayThucHien) = (SELECT YEAR(CONVERT(VARCHAR(10), getdate() - 1, 101)))) 'LKML'";
                try
                {
                    sanluong = db.Database.SqlQuery<SanLuong>(sql).ToList<SanLuong>()[0];
                }
                catch (Exception e)
                {

                }

            }

            ViewBag.soLuotHuyDong = soLuotHuyDong;
            ViewBag.hetHanChungChi = hetHanChungChi;
            ViewBag.vuTaiNan = vuTaiNan;
            ViewBag.nghiVLD = nghiVLD;
            ViewBag.tren82 = tren82;
            ViewBag.duoi82 = duoi82;
            ViewBag.listNghiVLD = listNghiVLD;
            ViewBag.listNhanLuc = listNhanLuc;
            ViewBag.sanluong = sanluong;
            return View("/Views/TCLD/bao-cao-nhanh.cshtml");
        }

        /// <summary>
        /// The GetData
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [Route("phong-tcld/get-data")]
        [HttpPost]
        public ActionResult GetData()
        {
            try
            {
                string date = Request["date"];
                date = date.Split('/')[2] + "/" + date.Split('/')[1] + "/" + date.Split('/')[0];
                int soLuotHuyDong = 0;
                int vuTaiNan = 0;
                int nghiVLD = 0;
                int hetHanChungChi = 0;
                int tren82 = 0;
                int duoi82 = 0;
                List<NghiVLD> listNghiVLD = new List<NghiVLD>();
                List<NhanLuc> listNhanLuc = new List<NhanLuc>();
                SanLuong sanluong = new SanLuong();
                using (QUANGHANHABCEntities db = new QUANGHANHABCEntities())
                {
                    ////////////////////////////GET so luot huy dong////////////////////////////////

                    db.Configuration.LazyLoadingEnabled = false;
                    string sql = "select (case when count(MaQuyetDinh) is null then 0 else count(MaQuyetDinh) end ) as SoLuotHuyDong from quyetdinh\n" +
                                "where maquyetdinh in\n" +
                                "(SELECT  distinct dd.MaQuyetDinh FROM DIEUDONG_NHANVIEN dd,QuyetDinh qd where dd.MaQuyetDinh=qd.MaQuyetDinh and qd.SoQuyetDinh<>'' )\n" +
                                "AND NgayQuyetDinh = @NgayQuyetDinh";
                    try
                    {
                        soLuotHuyDong = db.Database.SqlQuery<int>(sql,
                                                new SqlParameter("NgayQuyetDinh", DateTime.Parse(date))).ToList<int>()[0];
                    }
                    catch (Exception e)
                    {

                    }


                    ////////////////////////////GET SO LUONG TAI NAN//////////////////////////////
                    sql = "select (case when Count(tn.MaNV) is null then 0 else Count(tn.MaNV) end )  from \n" +
                          "(select MaNV, Ngay from TaiNan where\n" +
                          "Ngay = @NgayQuyetDinh) as tn";
                    try
                    {
                        vuTaiNan = db.Database.SqlQuery<int>(sql,
                       new SqlParameter("NgayQuyetDinh", DateTime.Parse(date))).ToList<int>()[0];
                    }
                    catch (Exception e)
                    {

                    }

                    //////////////////////////////////////////////////////////////////////////////

                    /// ////////////////////////////GET SO LUONG HET HAN CC//////////////////////////////
                    sql = "select (case when sum(th.st)  is null then 0 else sum(th.st) end ) \n" +
                          "from(select cn.MaNV, cn.NgayCap, cc.ThoiHan, (case\n" +
                          "when DATEADD(MONTH, cc.ThoiHan, cn.NgayCap) <= @NgayQuyetDinh\n" +
                          "then 1 else 0 end) as st\n" +
                          "from ChungChi_NhanVien cn join ChungChi cc on cn.MaChungChi = cc.MaChungChi) as th";
                    try
                    {
                        hetHanChungChi = db.Database.SqlQuery<int>(sql, new SqlParameter("NgayQuyetDinh", DateTime.Parse(date))).ToList<int>()[0];
                    }
                    catch (Exception e)
                    {

                    }


                    //////////////////////////////////////////////////////////////////////////////

                    /// ////////////////////////////GET SO LUONG NGHI VLD//////////////////////////////
                    sql = @"select case when Count(b.MaNV) is null then 0 else count(b.MaNV) end 'SoLuongNhanVien' from Header_DiemDanh_NangSuat_LaoDong a join DiemDanh_NangSuatLaoDong b on a.HeaderID = b.HeaderID
                            where a.NgayDiemDanh = (SELECT CONVERT(VARCHAR(10), @NgayDiemDanh, 101)) and b.LyDoVangMat = N'Vô lý do'
                            group by a.NgayDiemDanh, b.LyDoVangMat";
                    try
                    {
                        nghiVLD = db.Database.SqlQuery<int>(sql,
                                        new SqlParameter("NgayDiemDanh", DateTime.Parse(date))).ToList<int>()[0];
                    }
                    catch (Exception e)
                    {

                    }


                    //////////////////////////////////////////////////////////////////////////////

                    //////////////////////////////////////GET TI LE HUY DONG////////////////////////////////////////
                    string tempDate = date.Split('/')[0] + "/" + date.Split('/')[1] + "/" + date.Split('/')[2];
                    try
                    {
                        sql = @"select a.department_id, a.QL, (a.KT + a.CD) as Tong, a.KT, a.CD, 0 as 'HSTT', 
                                a.dilam, (a.vld + a.om + a.khac + a.phep) as vang, 
                                a.vld,a.om,a.phep,a.khac, 
                                (case when (a.KT+ a.CD) = 0 then 0 else round(Convert(float,(a.KT + a.CD - a.tong_nghidai)-(a.vld + a.om + a.khac + a.phep))/(a.KT + a.CD - a.tong_nghidai)*100,1) end) as tile, 
                                b.than, b.metlo, b.xen,b.diemluong,a.tong_nghidai,a.nghidai_om,a.nghidai_thld,a.nghidai_vld 
                                from 
                                (select a.department_id, a.QL, a.KT, a.CD, 
                                sum(case when d.DiLam = 1 and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as dilam, 
                                sum(case when d.DiLam = 0  and d.LyDoVangMat like N'Vô lý do' and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'vld', 
                                sum(case when d.DiLam = 0  and d.LyDoVangMat like N'Ốm' and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'om', 
                                sum(case when d.DiLam = 0  and d.LyDoVangMat like N'Nghỉ phép' and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'phep', 
                                sum(case when d.DiLam = 0  and d.LyDoVangMat like N'Khác' and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'khac', 
                                SUM(case when d.LyDoVangMat in (N'Tai nạn lao động',N'Ốm dài',N'Thai sản',N'Tạm hoãn lao động',N'Vô lý do dài') and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'tong_nghidai', 
                                SUM(case when d.LyDoVangMat in (N'Vô lý do dài') and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'nghidai_vld', 
                                SUM(case when d.LyDoVangMat in (N'Tạm hoãn lao động') and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'nghidai_thld', 
                                SUM(case when d.LyDoVangMat in (N'Ốm dài') and h.NgayDiemDanh = @NgayDiemDanh then 1 else 0 end) as 'nghidai_om' 
                                 from(select a.department_id, 
                                sum(case when ncv.MaNhomCongViec = 10 then  1 else 0 end) as QL, 
                                sum(case when ncv.MaNhomCongViec = 6 then  1 else 0 end) as KT, 
                                sum(case when ncv.MaNhomCongViec = 7 then  1 else 0 end) as CD 
                                from Department a left outer join NhanVien n on n.MaPhongBan = a.department_id 
                                join CongViec_NhomCongViec cn on n.MaCongViec = cn.MaCongViec 
                                join NhomCongViec ncv on cn.MaNhomCongViec = ncv.MaNhomCongViec 
                                where a.department_type like N'%chính%' and a.department_id != 'PXST' and a.department_id != 'PXLT' 
                                group by a.department_id) 
                                 as a left outer join Header_DiemDanh_NangSuat_LaoDong h 
                                on a.department_id = h.MaPhongBan left outer join DiemDanh_NangSuatLaoDong d 
                                on h.HeaderID = d.HeaderID 
                                group by a.department_id, a.QL, a.KT, a.CD) as a inner join 
                                ( select a.department_id, 
                                sum(case when h.ThanThucHien is not null and h.NgayDiemDanh = @NgayDiemDanh then h.ThanThucHien else 0 end) as 'than', 
                                sum(case when h.MetLoThucHien is not null and h.NgayDiemDanh = @NgayDiemDanh then h.MetLoThucHien else 0 end) as 'metlo', 
                                sum(case when h.XenThucHien is not null and h.NgayDiemDanh = @NgayDiemDanh then h.XenThucHien else 0 end) as 'xen', 
                                sum(case when h.TotalEffort is not null and h.NgayDiemDanh = @NgayDiemDanh then h.TotalEffort else 0 end) as 'diemluong' 
                                from Department a left outer join Header_DiemDanh_NangSuat_LaoDong h 
                                on a.department_id = h.MaPhongBan 
                                group by a.department_id 
                                ) as b on a.department_id = b.department_id";
                        List<BaoCaoNgayDB> listTLHD = db.Database.SqlQuery<BaoCaoNgayDB>(sql, new SqlParameter("NgayDiemDanh", tempDate)).ToList();
                        for (int i = 0; i < listTLHD.Count; i++)
                         {
                            if (listTLHD[i].tile >= 82)
                            {
                                tren82++;
                            }
                            else
                            {
                                duoi82++;
                            }
                        }
                    }
                    catch (Exception e)
                    {

                    }

                    ////////////////////////////////////////////////////////////////////////////////////////////////

                    //////////////////////////////////////GET NV NGHI VLD////////////////////////////////////////
                    sql = "select n.MaNV, n.Ten as HoTen,Department.department_name as TenDonVi\n" +
                           "from Department, Header_DiemDanh_NangSuat_LaoDong hd inner join DiemDanh_NangSuatLaoDong d\n" +
                           "on hd.HeaderID = d.HeaderID and hd.NgayDiemDanh = @NgayDiemDanh and d.LyDoVangMat like N'Vô lý do' inner join NhanVien n\n" +
                           "on d.MaNV = n.MaNV\n" +
                           "where Department.department_id = hd.MaPhongBan";
                    try
                    {
                        listNghiVLD = db.Database.SqlQuery<NghiVLD>(sql, new SqlParameter("NgayDiemDanh", date)).ToList<NghiVLD>();
                    }
                    catch (Exception e)
                    {

                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////////////
                    
                    ////////////////////////////////////////GET DATA NHAN LUC////////////////////////////////////////////////
                    sql = "select tb1.department_id as MaDonVi,\n" +
                            "(case when tb2.soluong is null then 0 else tb2.soluong end) as SoLuong\n" +
                            "from\n" +
                            "(select * from Department where department_id in\n" +
                            "('PXKT1', 'PXKT2', 'PXKT3', 'PXKT4', 'PXKT5', 'PXKT6', 'PXKT7', 'PXKT8', 'PXKT9', 'PXKT10', 'PXKT11',\n" +
                            "'PXDL3', 'PXDL5', 'PXDL7', 'PXDL8', 'PXVT1', 'PXVT2')) tb1\n" +
                            "left join\n" +
                            "(select hd.MaPhongBan, count(d.MaNV) as soluong from Header_DiemDanh_NangSuat_LaoDong hd inner\n" +
                             "                                               join DiemDanh_NangSuatLaoDong d\n" +
                            "on hd.HeaderID = d.HeaderID\n" +
                            "where hd.NgayDiemDanh = @NgayDiemDanh and DiLam = 1\n" +
                            "group by hd.MaPhongBan) tb2\n" +
                            "on tb1.department_id = tb2.MaPhongBan\n" +
                            "group by tb1.department_id,tb2.soluong";
                    try
                    {
                        listNhanLuc = db.Database.SqlQuery<NhanLuc>(sql, new SqlParameter("NgayDiemDanh", date)).ToList<NhanLuc>();
                    }
                    catch (Exception e)
                    {

                    }
                    ///////////////////////////////////////GET DATA SAN LUONG///////////////////////////////////////////////
                    sql = "select (select (case when sum(tc_kh.SanLuongKeHoach) is null then 0 else sum(tc_kh.SanLuongKeHoach) end) from (select tc.MaTieuChi, tc.DonViDo, kh.SanLuongKeHoach, kh.ThangKeHoach, kh.NamKeHoach from KeHoach_TieuChi kh join TieuChi tc on kh.MaTieuChi = tc.MaTieuChi) as tc_kh where tc_kh.MaTieuChi in (1,2,3,4) and  tc_kh.ThangKeHoach = @Thang1 and tc_kh.NamKeHoach = @Nam1) 'SLKH',\n" +
                        "(select(case when sum(tc_kh.SanLuongKeHoach) is null then 0 else sum(tc_kh.SanLuongKeHoach) end) from(select tc.MaTieuChi, tc.DonViDo, kh.SanLuongKeHoach, kh.ThangKeHoach, kh.NamKeHoach from KeHoach_TieuChi kh join TieuChi tc on kh.MaTieuChi = tc.MaTieuChi) as tc_kh where tc_kh.MaTieuChi in (7, 8) and tc_kh.ThangKeHoach = @Thang2 and tc_kh.NamKeHoach = @Nam2) 'MLKH',\n" +
                        "(select(case when sum(tc_th.SanLuongThucHien) is null then 0 else sum(tc_th.SanLuongThucHien) end) from(select tc.MaTieuChi, tc.DonViDo, th.SanLuongThucHien, th.NgayThucHien from ThucHien_TieuChi th join TieuChi tc on th.MaTieuChi = tc.MaTieuChi) as tc_th where tc_th.MaTieuChi in (1, 2, 3, 4) and MONTH(tc_th.NgayThucHien) = @Thang3  and YEAR(tc_th.NgayThucHien) = @Nam3) 'LKSL',\n" +
                        "(select(case when sum(tc_th.SanLuongThucHien) is null then 0 else sum(tc_th.SanLuongThucHien) end) from(select tc.MaTieuChi, tc.DonViDo, th.SanLuongThucHien, th.NgayThucHien from ThucHien_TieuChi th join TieuChi tc on th.MaTieuChi = tc.MaTieuChi) as tc_th where tc_th.MaTieuChi in (7, 8) and MONTH(tc_th.NgayThucHien) = @Thang4  and YEAR(tc_th.NgayThucHien) = @Nam4) 'LKML'";

                    try
                    {
                        sanluong = db.Database.SqlQuery<SanLuong>(sql,
                                                new SqlParameter("Thang1", date.Split('/')[1]),
                                                new SqlParameter("Thang2", date.Split('/')[1]),
                                                new SqlParameter("Thang3", date.Split('/')[1]),
                                                new SqlParameter("Thang4", date.Split('/')[1]),
                                                new SqlParameter("Nam1", date.Split('/')[0]),
                                                new SqlParameter("Nam2", date.Split('/')[0]),
                                                new SqlParameter("Nam3", date.Split('/')[0]),
                                                new SqlParameter("Nam4", date.Split('/')[0])
                                                ).ToList<SanLuong>()[0];
                    }
                    catch (Exception e)
                    {

                    }

                }
                return Json(new { success = true, tren82 = tren82, duoi82 = duoi82, soLuongHuyDong = soLuotHuyDong, vuTaiNan = vuTaiNan, nghiVLD = nghiVLD, hetHanChungChi = hetHanChungChi, listNghiVLD = listNghiVLD, listNhanLuc = listNhanLuc, sanluong = sanluong }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Lỗi" }, JsonRequestBehavior.AllowGet);
            }
        }

        //[Route("phong-tcld/bao-cao-nhanh-lao-dong-tien-luong-vtl1")]
        //public ActionResult DetailReport()
        //{
        //    ViewBag.nameDepartment = "baocao-sanluon-laodong";
        //    return View("/Views/TCLD/bao_cao_nhanh_tung_phan_xuong.cshtml");
        //}
        /// <summary>
        /// The Report1
        /// </summary>
        /// <param name="ca">The ca<see cref="string"/></param>
        /// <param name="donvi">The donvi<see cref="string"/></param>
        /// <param name="date">The date<see cref="string"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [Route("phong-tcld/bao-cao-chi-tiet-theo-ca")]
        public ActionResult Report1(string ca, string donvi, string date)
        {
            if (ca == null)
            {
                ca = "1";
            }
            if (date == null)
            {
                date = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
            }
            if (donvi == null)
            {
                donvi = "DL1";
            }
            ViewBag.nameDepartment = "baocao-sanluon-laodong";
            ViewBag.ca = ca;
            ViewBag.donvi = donvi;
            ViewBag.date = date;
            return View("/Views/TCLD/bao_cao_chi_tiet_theo_ca.cshtml");
        }

        /// <summary>
        /// The List
        /// </summary>
        /// <param name="ca">The ca<see cref="string"/></param>
        /// <param name="donvi">The donvi<see cref="string"/></param>
        /// <param name="date">The date<see cref="string"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [Route("phong-tcld/bao-cao-chi-tiet-theo-ca")]
        [HttpPost]
        public ActionResult List(string ca, string donvi, string date)
        {
            if (ca == "CA 1" || ca == null)
            {
                ca = "1";
            }
            if (ca == "CA 2")
            {
                ca = "2";
            }
            if (ca == "CA 3")
            {
                ca = "3";
            }
            if (date == null)
            {
                date = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
            }
            if (donvi == null)
            {
                donvi = "DL1";
            }
            var calamviec = Convert.ToInt32(ca);
            var datesql = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            using (QUANGHANHABCEntities db = new QUANGHANHABCEntities())
            {
                List<DiemDanh_NangSuatLaoDong> list = new List<DiemDanh_NangSuatLaoDong>();
                List<BaoCaoTheoCa> customNSLDs = new List<BaoCaoTheoCa>();
                BaoCaoTheoCa cus;
                int stt = 1;
                foreach (var i in list)
                {
                    cus = new BaoCaoTheoCa
                    {
                        //ID = stt,
                        Name = db.NhanViens.Where(a => a.MaNV == i.MaNV).First().Ten,
                        BacTho = db.NhanViens.Where(a => a.MaNV == i.MaNV).First().BacLuong,
                        ChucDanh = db.NhanViens.Where(a => a.MaNV == i.MaNV).First().CongViec == null ? "" : db.NhanViens.Where(a => a.MaNV == i.MaNV).First().CongViec.TenCongViec,
                        DuBaoNguyCo = i.DuBaoNguyCo,
                        HeSoChiaLuong = i.HeSoChiaLuong.ToString(),
                        LuongSauDuyet = i.DiemLuong.ToString(),
                        LuongTruocDuyet = i.DiemLuong.ToString(),
                        NoiDungCongViec = db.Departments.First().department_name,
                        SoThe = i.MaNV,
                        YeuCauBPKTAT = i.GiaiPhapNguyCo
                    };
                    customNSLDs.Add(cus);
                    stt++;
                }
                ViewBag.nsld = customNSLDs;
                var js = Json(new { success = true, data = customNSLDs }, JsonRequestBehavior.AllowGet);
                var dataserialize = new JavaScriptSerializer().Serialize(js.Data);
                return js;
            }
        }
    }

    /// <summary>
    /// Defines the <see cref="NghiVLD" />
    /// </summary>
    public class NhanLuc
    {
        public string MaDonVi { get; set; }
        public int SoLuong { get; set; }
    }
    public class NghiVLD
    {
        /// <summary>
        /// Gets or sets the MaNV
        /// </summary>
        public string MaNV { get; set; }

        /// <summary>
        /// Gets or sets the HoTen
        /// </summary>
        public string HoTen { get; set; }

        /// <summary>
        /// Gets or sets the TenDonVi
        /// </summary>
        public string TenDonVi { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NghiVLD"/> class.
        /// </summary>
        public NghiVLD()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NghiVLD"/> class.
        /// </summary>
        /// <param name="maNV">The maNV<see cref="string"/></param>
        /// <param name="hoTen">The hoTen<see cref="string"/></param>
        /// <param name="tenDonVi">The tenDonVi<see cref="string"/></param>
        public NghiVLD(string maNV, string hoTen, string tenDonVi)
        {
            MaNV = maNV;
            HoTen = hoTen;
            TenDonVi = tenDonVi;
        }

    }

    public class SanLuong
    {
        public double? SLKH { get; set; }
        public double? MLKH { get; set; }
        public double? LKSL { get; set; }
        public double? LKML { get; set; }
    }
}
