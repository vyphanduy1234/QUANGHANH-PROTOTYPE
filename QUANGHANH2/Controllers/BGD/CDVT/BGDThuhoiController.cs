﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;using System.Web.Routing;

namespace QUANGHANHCORE.Controllers.CDVT.Report
{
    public class BGDThuhoiController : Controller
    {
        [Route("ban-giam-doc/bao-cao-thu-hoi")]
        public ActionResult Index()
        {
            return View("/Views/BGD/CDVT/bao_cao_thu_hoi.cshtml");
        }
        [Route("ban-giam-doc/bao-cao-thu-hoi/excel")]
        public ActionResult Export()
        {
            return File( "~/excel/thuhoi.xls", contentType: "text/plain; charset=utf-8", fileDownloadName: "thuhoi.xls");
        }
    }
}