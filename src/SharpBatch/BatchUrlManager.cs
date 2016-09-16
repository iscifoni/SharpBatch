using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SharpBatch
{
    public enum BatchUrlManagerCommand
    {
        Exec,
        Status
    }

    public class BatchUrlManager
    {
        private const string BaseBatch = "/batch";

        public BatchUrlManager()
        {
        }

        public BatchUrlManager(string BatchName, string BatchActionName)
        {
            isBatch = true;
            RequestBatchAction = BatchActionName;
            RequestBatchName = BatchName;
        }
        public BatchUrlManager(PathString path)
        {
            PathString batchUrlDetail;
            if (path.StartsWithSegments(new PathString(BaseBatch), out batchUrlDetail))
            {
                isBatch = true;

                var batchCallPathVector = batchUrlDetail.Value.Split('/');
                if (batchCallPathVector.Length >= 3)
                {
                    BatchUrlManagerCommand UrlManagerCommandEnumParser;
                    if(!Enum.TryParse<BatchUrlManagerCommand>(batchCallPathVector[1] , true , out UrlManagerCommandEnumParser))
                    {
                        throw new InvalidCastException($"Command {batchCallPathVector[1]} not found");
                    }

                    RequestCommand = UrlManagerCommandEnumParser;
                    RequestBatchName = batchCallPathVector[2];

                    //only for Exec command we have action to execute
                    RequestBatchAction = RequestCommand == BatchUrlManagerCommand.Exec ? batchCallPathVector[3] : null;
                    
                }
            }
        }

        public bool isBatch { get; private set; }
        public string RequestBatchName { get; private set; }
        public string RequestBatchAction { get; private set; }
        public BatchUrlManagerCommand RequestCommand { get; private set; }

    }
}
