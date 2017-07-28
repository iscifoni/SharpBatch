using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharpBatch.Tracking.Abstraction;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SharpBatch.Web.Controllers
{
    public class HomeController : Controller
    {
        private ISharpBatchTracking _trackingProvider;

        public HomeController(ISharpBatchTracking trackingProvider, ILogger<HomeController> logger)
        {
            _trackingProvider = trackingProvider;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<bool> LoadFakeDataInDB()
        {
            var sessionID = Guid.NewGuid();

            await _trackingProvider.StartAsync("Batch1", sessionID);
            await _trackingProvider.PingAsync(sessionID);
            await _trackingProvider.StopAsync(sessionID);

            sessionID = Guid.NewGuid();
            await _trackingProvider.StartAsync("Batch2", sessionID);
            await _trackingProvider.PingAsync(sessionID);
            await _trackingProvider.PingAsync(sessionID);
            await _trackingProvider.PingAsync(sessionID);
            await _trackingProvider.PingAsync(sessionID);
            await _trackingProvider.StopAsync(sessionID);

            sessionID = Guid.NewGuid();
            await _trackingProvider.StartAsync("Batch2", sessionID);
            await _trackingProvider.PingAsync(sessionID);
            await _trackingProvider.PingAsync(sessionID);
            await _trackingProvider.AddMessageAsync(sessionID, "Messaggio 1 Messaggio 1 Messaggio 1 Messaggio 1 Messaggio 1 Messaggio 1 Messaggio 1 Messaggio 1 Messaggio 1 Messaggio 1");
            await _trackingProvider.StopAsync(sessionID);

            sessionID = Guid.NewGuid();
            await _trackingProvider.StartAsync("Batch2", sessionID);
            await _trackingProvider.PingAsync(sessionID);
            await _trackingProvider.PingAsync(sessionID);
            await _trackingProvider.AddMessageAsync(sessionID, "Messaggio 1 Messaggio 1 Messaggio 1 Messaggio 1 Messaggio 1 Messaggio 1 Messaggio 1 Messaggio 1 Messaggio 1 Messaggio 1");
            await _trackingProvider.AddExAsync(sessionID, new Exception("Errore Errore Errore Errore Errore Errore Errore Errore "));
            await _trackingProvider.StopAsync(sessionID);

            return true;
        }
       
    }
}
