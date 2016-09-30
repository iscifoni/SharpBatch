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
using SharpBatch;

namespace SharpBatchTest
{
    public class Batchs
    {
    }

    public class SimplePOCOBatch
    {
        public string method1()
        {
            return "method1";
        }

        public int method2()
        {
            return 123;
        }

        public string method3(string param1)
        {
            return param1;
        }
    }

    public class InferitFromSimplePOCO : SimplePOCOBatch { }

    [Batch()]
    public class BatchFromAttribute
    {
        public string method1()
        {
            return "method1";
        }

        public int method2()
        {
            return 123;
        }
    }

    public class InheritFromBatchAttribute : BatchFromAttribute { }

    [Batch("BatchName")]
    public class SimplePOCOConfgigurationAttributeBatch
    {
        public string method1()
        {
            return "method1";
        }
    }

    public class SimplePOCOPropertyBatch
    {
        [BatchContext]
        public ContextInvoker context { get; set; }
    }

    public class SimplePOCOContructorParamsBatch
    {
        SimplePOCOBatch _simplePocoBatch;
        public SimplePOCOContructorParamsBatch(SimplePOCOBatch simplePocoBatch)
        {
            _simplePocoBatch = simplePocoBatch;
        }
    }

}
