using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnostics.Windows;
using BenchmarkDotNet.Running;
using SharpBatch.internals;


namespace SharpBatchBenchmark
{
    [Config(typeof(Config))]
    public class batchConfigurationDictionaryBenchmark
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                Add(new MemoryDiagnoser());
                Add(new InliningDiagnoser());
            }
        }

        [Benchmark(Baseline = true)]
        public bool batchConfigurationDictionary()
        {
            batchConfigurationDictionary dict = new SharpBatch.internals.batchConfigurationDictionary();
            dict.AddOrUpdate("key1", "value0");
            dict.AddOrUpdate("key1", "value0");

            return true;
        }

        [Benchmark]
        public bool batchConfigurationDictionaryRefactoring()
        {
            batchConfigurationDictionary dict = new SharpBatch.internals.batchConfigurationDictionary();
            dict.AddOrUpdate("key1", "value0");
            dict.AddOrUpdate("key1", "value0");

            return true;
        }

    }
}
