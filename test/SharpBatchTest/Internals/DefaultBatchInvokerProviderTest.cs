using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using SharpBatch;
using SharpBatch.internals;
using SharpBatch.Tracking.Memory;
using Xunit;

namespace SharpBatchTest.Internals
{
    public class DefaultBatchInvokerProviderTest
    {
        [Fact]
        public async void DefaultBatchInvokerProviderTest_InvokeAsync()
        {
            //Arrange
            var batchInvoker = new Mock<IBatchInvoker>(MockBehavior.Strict);
            var sharpBatchTrakingFactory = new Mock<ISharpBatchTrackingFactory>(MockBehavior.Strict);
            sharpBatchTrakingFactory.Setup((s) => s.getTrakingProvider())
                .Returns(new TrackingMemory())
                .Verifiable();

            var defaultBatchInvokerProvider = new DefaultBatchInvokerProvider(batchInvoker.Object, sharpBatchTrakingFactory.Object);
            batchInvoker.Setup((s) => s.InvokeAsync(It.IsAny<ContextInvoker>()))
                .Returns(Task.FromResult<object>("executed"))
                .Verifiable();

            var requestService = new Mock<IServiceProvider>(MockBehavior.Strict);
            requestService.Setup((s) => s.GetService(It.IsAny<Type>()))
                .Returns((object)"Any")
                .Verifiable();

            var httpRequest = new Mock<HttpRequest>(MockBehavior.Strict);
            httpRequest.Setup((s) => s.Path)
                .Returns(new PathString("/batch/exec/batch/action"));
            httpRequest.Setup((s) => s.QueryString)
                .Returns(new QueryString())
                .Verifiable();

            Stream bodyStream = new MemoryStream();
            var httpResponse = new Mock<HttpResponse>(MockBehavior.Strict);
            httpResponse.Setup((s) => s.Body).Returns(bodyStream).Verifiable();

            var context = new Mock<HttpContext>(MockBehavior.Strict);
            context.Setup((s) => s.Request).Returns(httpRequest.Object).Verifiable();
            context.Setup((s) => s.Response).Returns(httpResponse.Object).Verifiable();
            context.Setup((s) => s.RequestServices).Returns(requestService.Object).Verifiable();

            var contextInvoker = ContextInvoker.Create(context.Object);
            contextInvoker.SessionId = Guid.NewGuid();

            //Act
            var response = await defaultBatchInvokerProvider.InvokeAsync(contextInvoker);

            //Assert
            Assert.NotNull(response);
            Assert.Equal("executed", response);
        }
    }
}
