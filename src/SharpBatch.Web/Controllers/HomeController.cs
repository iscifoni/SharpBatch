//Copyright 2016 Scifoni Ivano
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

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
