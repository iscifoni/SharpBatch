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
        ISharpBatchTracking _sharpBatchTraking;

        public TrackingController(ISharpBatchTracking sharpBatchTraking)
        {
            _sharpBatchTraking = sharpBatchTraking; 
        }   

        [HttpGet]
        public IActionResult getRunningBatch()
        {
            return View(_sharpBatchTraking.GetRunning());
        }
    }
}
