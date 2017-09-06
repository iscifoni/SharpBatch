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
using System.Reflection;
using System.Threading.Tasks;
using SharpBatch;
using SharpBatch.internals;
using SharpBatch.Serialization.Abstract;
using Moq;
using Xunit;

namespace SharpBatchTest.Internals
{
    public class DefaultBatchInvokerParameterBindingTest
    {
        //ToDo : array parameter value

        [Fact]
        public void DefaultBatchInvokerParameterBindingTest_Bind()
        {
            //Arange
            List<TypeInfo> types = new List<TypeInfo>();
            types.Add(typeof(SimplePOCOBatch).GetTypeInfo());
            var descriptions = AssemblyDiscoveryActionDescription.actionDescription(types);

            var batchParameterDictionary = new BatchParameterDictionary();
            batchParameterDictionary.Add("param1", "first param");

            var serializer = new Mock<IModelSerializer>(MockBehavior.Strict);

            var batchInvokerParameterBinding = new DefaultBatchInvokerParameterBinding(batchParameterDictionary, descriptions.Single((p)=>p.ActionName.Equals("method3")).ActionInfo, serializer.Object);

            //Act
            var response = batchInvokerParameterBinding.Bind();

            //Assert
            Assert.NotNull(response);
            Assert.IsType<object[]>(response);
            Assert.Equal(1, response.Length);
            Assert.Equal("first param", response[0]);

        }


    }
}
