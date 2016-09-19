using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public static class TaskWrapper
    {
#if NET451
        public static readonly Task CompletedTask = Task.FromResult(0);
#else
        public static readonly Task CompletedTask = Task.CompletedTask;
#endif
    }
}
