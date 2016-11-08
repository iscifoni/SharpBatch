using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharpBatch.Traking.Abstraction;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SharpBatch.Web.Controllers
{
    public class HomeController : Controller
    {
        private ISharpBatchTraking _trackingProvider;

        public HomeController(ISharpBatchTraking trackingProvider)
        {
            _trackingProvider = trackingProvider;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RunningBatchPartial()
        {
            var model = _trackingProvider.GetRunning();
            return PartialView(model);
        }
    }
}
