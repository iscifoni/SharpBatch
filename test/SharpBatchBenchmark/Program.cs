using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace SharpBatchBenchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<batchConfigurationDictionaryBenchmark>();
            Console.ReadLine();
        }
    }
}
