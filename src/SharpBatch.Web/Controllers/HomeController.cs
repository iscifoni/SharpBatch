using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharpBatch.Tracking.Abstraction;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SharpBatch.Web.Controllers
{
    public class HomeController : Controller
    {
        private ISharpBatchTracking _trackingProvider;

        public HomeController(ISharpBatchTracking trackingProvider)
        {
            _trackingProvider = trackingProvider;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
