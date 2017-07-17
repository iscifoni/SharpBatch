using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SharpBatch.Web.ViewComponents
{
    public class BreadcrumbViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke(int maxPriority, bool isDone)
        {
            var path = HttpContext.Request.Path;
            var vPath = path.Value.Split('/');

            return View(vPath);
        }
    }
}
