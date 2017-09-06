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
using Microsoft.AspNetCore.Http;
using Moq;
using SharpBatch;
using SharpBatch.internals;
using Xunit;
using System.IO;

namespace SharpBatchTest.Internals
{
    public class DefaultBatchHandlerTest
    {
        [Fact]
        public async void DefaultBatchHandlerTest_invokeAsyncHttpContext()
        {
            //Arrange
            var batchActionFactory = new Mock<IBatchActionFactory>(MockBehavior.Strict);
            var batchActionProvider = new Mock<IBatchActionProvider>(MockBehavior.Strict);
            batchActionProvider.Setup((s) => s.InvokeAsync(It.IsAny<IBatchUrlManager>(), It.IsAny<ContextInvoker>()))
                .Returns(Task.FromResult<string>("finish"))
                .Verifiable();

            batchActionFactory.Setup((s) => s.getProvider(It.IsAny<IBatchUrlManager>())).Returns(batchActionProvider.Object).Verifiable();

            DefaultBatchHandler defaultBatchHandler = new DefaultBatchHandler(batchActionFactory.Object);

            var httpRequest = new Mock<HttpRequest>(MockBehavior.Strict);
            httpRequest.Setup((s) => s.Path).Returns(new PathString("/batch/exec/batch/action"));
            httpRequest.Setup((s) => s.QueryString).Returns(new QueryString()).Verifiable();

            Stream bodyStream = new MemoryStream();
            var httpResponse = new Mock<HttpResponse>(MockBehavior.Strict);
            httpResponse.Setup((s) => s.Body).Returns(bodyStream).Verifiable();

            var requestService = new Mock<IServiceProvider>(MockBehavior.Strict);

            var context = new Mock<HttpContext>(MockBehavior.Strict);
            context.Setup((s) => s.Request).Returns(httpRequest.Object).Verifiable();
            context.Setup((s) => s.Response).Returns(httpResponse.Object).Verifiable();
            context.Setup((s)=> s.RequestServices).Returns(requestService.Object).Verifiable();

            //Act
            await defaultBatchHandler.InvokeAsync(context.Object);

            bodyStream.Position = 0;
            string stringResponse;
            using (StreamReader sr = new StreamReader(bodyStream))
            {
                stringResponse = sr.ReadToEnd();
            }

            var response = stringResponse.Split('-');

            //Assert
            Assert.NotNull(response);
            Assert.IsType<string[]>(response);
            Assert.Equal(6, response.Length);
            Assert.Equal("finish", response[5].Trim());

            batchActionFactory.Verify();
            batchActionProvider.Verify();
            httpRequest.Verify();
            httpResponse.Verify();
            context.Verify();
        }

    }
}
