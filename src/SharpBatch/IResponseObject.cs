﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch
{
    public interface IResponseObject
    {
        object Response { get; set; }
        Guid SessionId { get; set; }
    }
}