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

            DefaultBatchInvoker defaultBatchInvoker = new DefaultBatchInvoker(
                propertyInvoker.Object, 
                methodActivator, 
                isharpBatchTrackingActivator.Object);

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
