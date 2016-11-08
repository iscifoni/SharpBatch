```ini

Host Process Environment Information:
BenchmarkDotNet.Core=v0.9.9.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4210M CPU 2.60GHz, ProcessorCount=4
Frequency=2533211 ticks, Resolution=394.7559 ns, Timer=TSC
CLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
GC=Concurrent Workstation
JitModules=clrjit-v4.6.1586.0

Type=batchConfigurationDictionaryBenchmark  Mode=Throughput  

```
                                  Method |    Median |    StdDev | Scaled | Scaled-SD |    Gen 0 | Gen 1 | Gen 2 | Bytes Allocated/Op |
---------------------------------------- |---------- |---------- |------- |---------- |--------- |------ |------ |------------------- |
            batchConfigurationDictionary | 1.3216 us | 0.1016 us |   1.00 |      0.00 | 3,417.00 |     - |     - |             280.81 |
 batchConfigurationDictionaryRefactoring | 1.2220 us | 0.0964 us |   0.95 |      0.10 | 3,218.12 |     - |     - |             264.44 |
