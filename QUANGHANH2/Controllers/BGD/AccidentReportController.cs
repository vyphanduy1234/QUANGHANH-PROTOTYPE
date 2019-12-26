﻿using QUANGHANH2.SupportClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;using System.Web.Routing;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QUANGHANHCORE.Controllers.BGD
{
    public class AccidentReportController : Controller
    {
        // GET: /<controller>/
        [Auther(RightID = "005")]
        [Route("ban-giam-doc/bao-cao-su-co")]
        public ActionResult Index()
        {
            return View("/Views/BGD/ReportIncident.cshtml");
        }
    }
}
