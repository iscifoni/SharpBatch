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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using SharpBatch;
using SharpBatch.internals;
using SharpBatch.Serialization.Abstract;
using SharpBatch.Tracking.Memory;
using Xunit;

namespace SharpBatchTest.Internals
{
    public class DefaultBatchInvokerTest
    {
        [Fact]
        public async void DefaultBatchInvokerTest_invokeAsync()
        {
            //Arrange
            var propertyInvoker = new Mock<IPropertyInvoker>(MockBehavior.Strict);
            //var methodActivator = new Mock<MethodActivator>(MockBehavior.Strict);
            //methodActivator.Setup((s) => s.CreateInstance<object>(It.IsAny<IServiceProvider>(), It.IsAny<Type>()))
            var methodActivator = new MethodActivator();

            var isharpBatchTrackingActivator = new Mock<ISharpBatchTrackingFactory>(MockBehavior.Strict);
            isharpBatchTrackingActivator.Setup((s) => s.getTrakingProvider()).Returns(new TrackingMemory()).Verifiable();

            var modelSerializer = new Mock<IModelSerializer>(MockBehavior.Strict);
            modelSerializer.Setup((s) => s.Serialize((object)"method1")).Returns("\"method1\"");

            DefaultBatchInvoker defaultBatchInvoker = new DefaultBatchInvoker(
                propertyInvoker.Object, 
                methodActivator, 
                isharpBatchTrackingActivator.Object,
                modelSerializer.Object);

            var requestService = new Mock<IServiceProvider>(MockBehavior.Strict);
            requestService.Setup((s) => s.GetService(It.IsAny<Type>())).Returns((object)"Any").Verifiable();

            var httpRequest = new Mock<HttpRequest>(MockBehavior.Strict);
            httpRequest.Setup((s) => s.Path).Returns(new PathString("/batch/exec/batch/action"));
            httpRequest.Setup((s) => s.QueryString).Returns(new QueryString()).Verifiable();

            Stream bodyStream = new MemoryStream();
            var httpResponse = new Mock<HttpResponse>(MockBehavior.Strict);
            httpResponse.Setup((s) => s.Body).Returns(bodyStream).Verifiable();

            var context = new Mock<HttpContext>(MockBehavior.Strict);
            context.Setup((s) => s.Request).Returns(httpRequest.Object).Verifiable();
            context.Setup((s) => s.Response).Returns(httpResponse.Object).Verifiable();
            context.Setup((s) => s.RequestServices).Returns(requestService.Object).Verifiable();

            ContextInvoker contextInvoker = ContextInvoker.Create(context.Object);

            List<TypeInfo> types = new List<TypeInfo>();
            types.Add(typeof(SimplePOCOBatch).GetTypeInfo());
            var descriptions = AssemblyDiscoveryActionDescription.actionDescription(types);

            contextInvoker.ActionDescriptor = descriptions.First();
            
            //Act
            var response = await defaultBatchInvoker.InvokeAsync(contextInvoker);

            //Assert
            Assert.NotNull(response);
            Assert.Equal("\"method1\"", response);
        }
    }
}
