﻿using QUANGHANH2.Models;
using QUANGHANH2.SupportClass;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using XCrypt;
using System.Linq.Dynamic;
using System.Data.SqlClient;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QUANGHANHCORE.Controllers
{

    public class LogInController : Controller
    {
        private QUANGHANHABCEntities db = new QUANGHANHABCEntities();
        // GET: /<controller>/ neww
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                string url = (string)Session["url"];
                return Redirect(url);
            }
            if (HttpContext.Request.Cookies["remme"] != null)
            {
                HttpCookie remme = HttpContext.Request.Cookies.Get("remme");
                login a = new login()
                {
                    username = remme.Values.Get("username"),
                    password = remme.Values.Get("password")
                };
                ViewBag.login = a;
                return View();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(string username, string password, string rm)
        {
            if (password == null) return RedirectToAction("Index");
            string passXc = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(password, "pl");
            var checkuser = db.Accounts.Where(x => x.Username == username).Where(y => y.Password == passXc).SingleOrDefault();
            if (checkuser != null)
            {
                if (checkuser.Username.Equals(username) && checkuser.Password.Equals(passXc))
                {
                    Session["UserID"] = checkuser.ID;
                    Session["time"] = DateTime.Now;
                    int id = checkuser.ID;
                    var Name = db.Database.SqlQuery<InfoAccount>(@"select a.ID,nv.Ten,a.Username,a.Position,c.TenCongViec,a.ADMIN,d.department_name,d.department_id from Account a , NhanVien nv , CongViec c , Department d
                                                        where a.NVID = nv.MaNV and c.MaCongViec = nv.MaCongViec and d.department_id = nv.MaPhongBan and a.ID = @id", new SqlParameter("id", id)).FirstOrDefault();
                    Session["departName"] = Name.department_name;
                    Session["departID"] = Name.department_id;
                    Session["account_id"] = Name.ID;
                    Session["Name"] = Name.Ten;
                    Session["username"] = Name.Username;
                    Session["Position"] = Name.Position;
                    Session["isAdmin"] = Name.ADMIN;
                    GetPermission(id);
                    if (!String.IsNullOrEmpty(rm))
                    {
                        if (rm.Equals("on"))
                        {
                            HttpCookie remme = new HttpCookie("remme");
                            remme["username"] = Name.Username;
                            remme["password"] = password;
                            remme.Expires = DateTime.Now.AddDays(365);
                            HttpContext.Response.Cookies.Add(remme);
                        }
                    }
                    if (Name.ADMIN) return RedirectToAction("Index", "ManagementUser");
                    string url = (string)Session["url"];
                    if (url == null)
                    {
                        ViewData["Notification"] = "Tài khoản chưa được kích hoạt";
                        Session.Abandon();
                        return View();
                    }
                    return Redirect(url);
                }
                else
                {
                    ViewData["Notification"] = "Tên đăng nhập/mật khẩu không đúng!";
                    return View();
                }
            }
            else
            {
                ViewData["Notification"] = "Tên đăng nhập/mật khẩu không đúng!";
                return View();
            }

        }
        [Route("Logout")]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }
        public void GetPermission(int UserID)
        {
            Session["UserID"] = UserID;
            List<string> RightIDs = new List<string>();
            var userRight = db.Account_Right_Detail.Where(a => a.AccountID == UserID).ToList();
            foreach (var right in userRight)
            {
                RightIDs.Add(right.RightID + "");
            }
            var user = db.Accounts.Where(x => x.ID == UserID).FirstOrDefault();

            if (user.NVID != null)
            {
                var url = db.NhanViens.Where(x => x.MaNV == user.NVID).FirstOrDefault();

                if (url.MaPhongBan.Equals("CDVT"))
                {
                    Session["url"] = "phong-cdvt";
                    RightIDs.Add("001");
                }
                if (url.MaPhongBan.Equals("TCLD"))
                {
                    Session["url"] = "phong-tcld";
                    RightIDs.Add("002");
                }
                if (url.MaPhongBan.Equals("KCS"))
                {
                    Session["url"] = "phong-kcs";
                    RightIDs.Add("003");
                }
                if (url.MaPhongBan.Equals("DK"))
                {
                    Session["url"] = "phong-dieu-khien";
                    RightIDs.Add("004");
                }
                if (url.MaPhongBan.Equals("BGD"))
                {
                    Session["url"] = "ban-giam-doc";
                    RightIDs.Add("005");
                }
                if (url.MaPhongBan.Contains("PX"))
                {
                    Session["url"] = "phan-xuong";
                    RightIDs.Add("006");
                }
                if (url.MaPhongBan.Equals("AT"))
                {
                    Session["url"] = "phong-an-toan/danh-sach-tai-nan";
                    RightIDs.Add("007");
                }
                if (url.MaPhongBan.Equals("KCM"))
                {
                    Session["url"] = "phong-kcm/ke-hoach-san-xuat-thang";
                    RightIDs.Add("008");
                }
            }
            if (Boolean.Parse(Session["isAdmin"].ToString()) == true)
            {
                RightIDs.Add("0");
                Session["url"] = "ManagementUser/Index";
                Session["departID"] = "QL";
            }
            RightIDs.Add("000");
            Session["RightIDs"] = RightIDs;
        }
        [Auther(RightID = "000")]
        public ActionResult Detail()
        {
            DateTime start = Convert.ToDateTime(Session["time"]).AddMinutes(20);
            ViewBag.remain = start.Subtract(DateTime.Now).TotalMinutes;
            return View("/Views/LogIn/Account_Information.cshtml");
        }
        [Auther(RightID = "000")]
        public JsonResult ResetPassword(string oldPass, string newPass, string rePass)
        {
            int id = Convert.ToInt32(Session["account_id"]);
            string passXc = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(oldPass, "pl");
            string rePasss = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(rePass, "pl");
            var user = db.Accounts.Where(x => x.ID == id).FirstOrDefault();
            if (string.IsNullOrEmpty(oldPass) || string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(rePass))
            {
                return Json(new Result()
                {
                    CodeError = 1,
                    Data = "Mật khẩu không được để trống"
                }, JsonRequestBehavior.AllowGet);
            }
            if (!newPass.Equals(rePass))
            {
                return Json(new Result()
                {
                    CodeError = 1,
                    Data = "2 mật khẩu không trùng khớp"
                }, JsonRequestBehavior.AllowGet);
            }
            if (!user.Password.Equals(passXc))
            {
                return Json(new Result()
                {
                    CodeError = 1,
                    Data = "Mật khẩu cũ không đúng"
                }, JsonRequestBehavior.AllowGet);
            }
            user.Password = rePasss;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new Result()
            {
                CodeError = 2,
                Data = "Thay đổi mật khẩu thành công"
            }, JsonRequestBehavior.AllowGet);
        }
    }
    public class login
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class Result
    {
        public int CodeError { get; set; }
        public string Data { get; set; }
    }
    public class InfoAccount
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public string Username { get; set; }
        public string Position { get; set; }
        public string TenCongViec { get; set; }
        public bool ADMIN { get; set; }
        public string department_name { get; set; }
        public string department_id { get; set; }
    }
}
