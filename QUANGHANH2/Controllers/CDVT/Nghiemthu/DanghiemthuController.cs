﻿using QUANGHANH2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Dynamic;
using System.Data.Entity;
using QUANGHANH2.SupportClass;
using System.Globalization;
using System.Data.SqlClient;

namespace QUANGHANHCORE.Controllers.CDVT.Nghiemthu
{
    public class DanghiemthuController : Controller
    {
        [Auther(RightID = "26")]
        [Route("phong-cdvt/da-nghiem-thu")]
        public ActionResult Index()
        {
            return View("/Views/CDVT/Nghiemthu/Danghiemthu.cshtml");
        }

        [Route("phong-cdvt/da-nghiem-thu/search")]
        [HttpPost]
        public ActionResult Search(string document_code, string equimentid, string equimentname, string date_start, string date_end)
        {
            //Server Side Parameter
            string requestID = Request["sessionId"];
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            //int length = 2;
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            List<Documentary_Extend> docList = new List<Documentary_Extend>();
            DateTime dstart;
            DateTime dend;
            try
            {
                if (date_start == "Nhập ngày bắt đầu (từ)" || date_start == "") date_start = "01/01/1900";
                dstart = DateTime.ParseExact(date_start, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (date_end == "Nhập ngày kết thúc (đến)" || date_end == "") dend = DateTime.Now;
                else dend = DateTime.ParseExact(date_end, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dend = dend.AddHours(23);
                dend = dend.AddMinutes(59);
            }
            catch
            {
                Response.Write("Vui lòng nhập đúng ngày tháng năm");
                return new HttpStatusCodeResult(400);
            }

            using (QUANGHANHABCEntities db = new QUANGHANHABCEntities())
            {
                string username = Session["Username"].ToString();
                Account account = new Account();
                account = db.Accounts.Where(x => x.Username.Equals(username)).FirstOrDefault<Account>();

                db.Configuration.LazyLoadingEnabled = false;
                string basesql = @"from Documentary d inner join DocumentaryType dt
on d.documentary_type = dt.documentary_type
inner join Acceptance a on a.documentary_id = d.documentary_id
inner join Equipment e on e.equipmentId = a.equipmentId
where d.documentary_code like @documentary_code and a.equipmentId like @equipmentId
and e.equipment_name like @equipment_name and a.acceptance_date between @dstart and @dend";
                docList = db.Database.SqlQuery<Documentary_Extend>(@"select d.date_created, d.person_created, d.documentary_id, a.equipmentId, e.equipment_name, a.acceptance_date, d.documentary_code, d.documentary_type, dt.documentary_name, dt.du_phong, dt.di_kem, dt.can
" + basesql + " order by " + sortColumnName + " " + sortDirection + " OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY",
new SqlParameter("documentary_code", "%" + document_code + "%"),
new SqlParameter("equipmentId", "%" + equimentid + "%"),
new SqlParameter("equipment_name", "%" + equimentname + "%"),
new SqlParameter("dstart", dstart),
new SqlParameter("dend", dend)).ToList();
                foreach (Documentary_Extend items in docList)
                {
                    items.QDQT = checkQDQT(items.documentary_id,account.ID);
                }
                //docList = db.Documentaries.ToList<Documentary>();
                int totalrows = db.Database.SqlQuery<int>(@"select count(d.documentary_id) " + basesql,
new SqlParameter("documentary_code", document_code),
new SqlParameter("equipmentId", equimentid),
new SqlParameter("equipment_name", equimentname),
new SqlParameter("dstart", dstart),
new SqlParameter("dend", dend)).FirstOrDefault();
                return Json(new { success = true, data = docList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrows }, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("phong-cdvt/da-nghiem-thu/quyet-dinh-quan-trong")]
        [HttpGet]
        public ActionResult AddQDQT(string docID)
        {
            QUANGHANHABCEntities dbContext = new QUANGHANHABCEntities();
            int id = Int32.Parse(Request["documentory_id"]);
            string username = Session["Username"].ToString();
            Account account = new Account();
            account = dbContext.Accounts.Where(x => x.Username.Equals(username)).FirstOrDefault<Account>();
            Important_Documentary checkDoc = new Important_Documentary();
            checkDoc = dbContext.Important_Documentary.Where(x => x.documentary_id == id && x.AccountID == account.ID).FirstOrDefault<Important_Documentary>();
            if (checkDoc == null)
            {
                Important_Documentary important_Documentary = new Important_Documentary();
                important_Documentary.documentary_id = id;
                important_Documentary.AccountID = account.ID;
                dbContext.Important_Documentary.Add(important_Documentary);
                dbContext.SaveChanges();
            }
            else
            {
                dbContext.Important_Documentary.Remove(checkDoc);
                dbContext.SaveChanges();
            }
            return Json(new { message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        public Boolean checkQDQT(int QDId,int accountID)
        {
            QUANGHANHABCEntities dbContext = new QUANGHANHABCEntities();
            Important_Documentary important_Documentary = new Important_Documentary();
            important_Documentary = dbContext.Important_Documentary.Where(x => x.documentary_id == QDId && x.AccountID == accountID).FirstOrDefault<Important_Documentary>();
            if (important_Documentary == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}