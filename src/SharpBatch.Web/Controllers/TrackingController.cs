using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharpBatch.Tracking.Abstraction;

namespace SharpBatch.Web.Controllers
{
    public class TrackingController:Controller
    {
        ISharpBatchTracking _trackingProvider;

        public TrackingController(ISharpBatchTracking trackingProvider)
        {
            _trackingProvider = trackingProvider; 
        }   

        [HttpGet]
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
