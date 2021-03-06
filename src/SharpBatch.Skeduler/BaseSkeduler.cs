﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharpBatch;
using SharpBatch.internals;

namespace SharpBatch.Skeduler
{
    public class BaseSkeduler
    {
        IList<BatchActionDescriptor> _batchActionDescription;
        IOptions<SkedulerSettings> _options;
        ILoggerFactory _loggerFactory;
        

        public BaseSkeduler(IApplicationBatchManager applicationBatchManager, 
                            IOptions<SkedulerSettings> options, 
                            ILoggerFactory loggerFactory)
        {
            _batchActionDescription = (from item in applicationBatchManager.BatchActions
                     from detail in item.ConfigureAttribute
                     where detail.GetType().Name == nameof(BatchSkedulerAttribute)
                     select item).ToList();

            _options = options;
            _loggerFactory = loggerFactory;
        }
        
        public void start()
        {
            Task.Run(async () =>
            {
                try
                {
                    var batchInvokerLogger = _loggerFactory.CreateLogger<BatchInvoker>();
                    
                    while (true)
                    {
                        var baseDateTime = DateTime.Now;
                        var batchList = _batchActionDescription.Where(p => {
                                                                                DateTime nextItemExecutionDate = p.GetNextExecutionDate();
                                                                                return nextItemExecutionDate <= baseDateTime;
                                                                            }
                                                                     ).ToList();

                        foreach(var item in batchList)
                        {
                            var tsk = BatchInvoker.invoke(item, _options, batchInvokerLogger);
                            item.Reskedule(baseDateTime);
                        }

                        await Task.Delay(1000);
                    }
                }
                catch(Exception ex)
                {
                    _loggerFactory.CreateLogger<BaseSkeduler>().LogError(ex, "Skeduler stopped. Error on executing skeduler");
                }
            });

        }
    }
}
