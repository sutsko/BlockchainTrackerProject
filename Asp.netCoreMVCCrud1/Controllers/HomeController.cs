using Asp.netCoreMVCCrud1.Models;
using System;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace Asp.netCoreMVCCrud1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjectContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public HomeController(ProjectContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET: Import
        public ActionResult Index()
        {
            return View();
        }



    }
}