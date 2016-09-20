using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch
{
    public class ResponseObject : IResponseObject
    {
        public ResponseObject()
        {

        }

        public ResponseObject(object response, Guid sessionID)
        {
            Response = response;
            SessionId = sessionID;
        }
        public object Response { get; set; }

        public Guid SessionId { get; set; }
    }
}
