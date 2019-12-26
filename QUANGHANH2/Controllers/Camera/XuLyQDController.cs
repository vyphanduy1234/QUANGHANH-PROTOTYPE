﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using QUANGHANH2.Models;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Threading;
using System.Globalization;
using System.Data.SqlClient;
using QUANGHANH2.SupportClass;

namespace QUANGHANH2.Controllers.Camera
{
    public class XuLyQDController : Controller
    {
        [Auther(RightID = "193")]
        [Route("camera/cap-nhat/quyet-dinh")]
        public ActionResult Index()
        {
            return View("/Views/Camera/DSQD.cshtml");
        }

        [Route("camera/cap-nhat/quyet-dinh/search")]
        [HttpPost]
        public ActionResult Search(string documentary_id, string type, string department, string reason, string dateStart, string dateEnd, string status)
        {
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            QUANGHANHABCEntities DBContext = new QUANGHANHABCEntities();
            List<DocumentaryDB> incidents;

            string query = "SELECT docu.*, docu.[out/in_come] as out_in_come, depa.department_name FROM Documentary docu inner join Department depa on docu.department_id = depa.department_id" +
                " where docu.documentary_code IS NOT NULL AND docu.documentary_status != 3 AND docu.documentary_type = 8 AND ";
            if (!documentary_id.Equals("") || !type.Equals("0") || !department.Equals("") || !reason.Equals("") || !status.Equals("0") || !(dateStart.Equals("") || dateEnd.Equals("")))
            {
                if (!dateStart.Equals("") && !dateEnd.Equals(""))
                {
                    DateTime dtStart = DateTime.ParseExact(dateStart, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime dtEnd = DateTime.ParseExact(dateEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dtEnd = dtEnd.AddHours(23);
                    dtEnd = dtEnd.AddMinutes(59);
                    query += "docu.date_created BETWEEN '" + dtStart + "' AND '" + dtEnd + "' AND ";
                }
                if (!documentary_id.Equals("")) query += "docu.documentary_code LIKE @documentary_id AND ";
                if (!type.Equals("0")) query += "docu.documentary_type LIKE @documentary_type AND ";
                if (!department.Equals("")) query += "depa.department_name LIKE @department_name AND ";
                if (!reason.Equals("")) query += "docu.reason LIKE @reason AND ";
                if (!status.Equals("0")) query += "docu.documentary_status = @documentary_status AND ";
            }
            query = query.Substring(0, query.Length - 5);
            incidents = DBContext.Database.SqlQuery<DocumentaryDB>(query,
                new SqlParameter("documentary_id", '%' + documentary_id + '%'),
                new SqlParameter("documentary_type", '%' + type + '%'),
                new SqlParameter("department_name", '%' + department + '%'),
                new SqlParameter("reason", '%' + reason + '%'),
                new SqlParameter("documentary_status", status)
                ).ToList();

            int totalrows = incidents.Count;
            int totalrowsafterfiltering = incidents.Count;
            //sorting
            incidents = incidents.OrderBy(sortColumnName + " " + sortDirection).ToList<DocumentaryDB>();
            //paging
            incidents = incidents.Skip(start).Take(length).ToList<DocumentaryDB>();
            foreach (DocumentaryDB item in incidents)
            {
                item.linkIdCode = new LinkIdCode();
                item.stringtype = "Sửa chữa";
                item.linkIdCode.link = "sua-chua";
                switch (item.documentary_status)
                {
                    case 1:
                        item.stringstatus = "Đang xử lý";
                        break;
                    case 2:
                        item.stringstatus = "Đã xử lý chưa nghiệm thu";
                        break;
                    case 3:
                        item.stringstatus = "Đã nghiệm thu";
                        break;
                }
                item.stringdate = item.date_created.ToString("dd/MM/yyyy");
                item.linkIdCode.code = item.documentary_code;
                item.linkIdCode.id = item.documentary_id;
            }
            return Json(new { success = true, data = incidents, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
    }
}