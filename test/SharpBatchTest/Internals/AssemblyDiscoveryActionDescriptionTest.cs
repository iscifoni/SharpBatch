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
using SharpBatch.internals;
using Xunit;

namespace SharpBatchTest.Internals
{
    public class AssemblyDiscoveryActionDescriptionTest
    {

        [Fact]
        public void AssemblyDiscoveryActionDescriptionTest_actionDescription_SiplePOCO()
        {
            //arrange
            List<TypeInfo> types = new List<TypeInfo>();
            types.Add(typeof(SimplePOCOBatch).GetTypeInfo());
            
            //act
            var response = AssemblyDiscoveryActionDescription.actionDescription(types.AsEnumerable());

            //assert
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.Equal(2, response.Count());
            Assert.Collection(response,
                (p) => {
                    Assert.Equal(p.BatchName, "SimplePOCO");
                    Assert.Equal(p.ActionName, "method1");
                    Assert.Equal(p.ReturnType, typeof(string));
                    },
                (p) => {
                    Assert.Equal(p.BatchName, "SimplePOCO");
                    Assert.Equal(p.ActionName, "method2");
                    Assert.Equal(p.ReturnType, typeof(int));
                }
            );
        }

        [Fact]
        public void AssemblyDiscoveryActionDescriptionTest_actionDescription_InheritsFromSimplePOCO()
        {
            //arrange
            List<TypeInfo> types = new List<TypeInfo>();
            types.Add(typeof(InferitFromSimplePOCO).GetTypeInfo());

            //act
            var response = AssemblyDiscoveryActionDescription.actionDescription(types.AsEnumerable());

            //assert
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.Equal(2, response.Count());
            Assert.Collection(response,
                (p) => {
                    Assert.Equal(p.BatchName, "InferitFromSimplePOCO");
                    Assert.Equal(p.ActionName, "method1");
                    Assert.Equal(p.ReturnType, typeof(string));
                },
                (p) => {
                    Assert.Equal(p.BatchName, "InferitFromSimplePOCO");
                    Assert.Equal(p.ActionName, "method2");
                    Assert.Equal(p.ReturnType, typeof(int));
                }
            );
        }

        [Fact]
        public void AssemblyDiscoveryActionDescriptionTest_actionDescription_BatchFromAttribute()
        {
            //arrange
            List<TypeInfo> types = new List<TypeInfo>();
            types.Add(typeof(BatchFromAttribute).GetTypeInfo());

            //act
            var response = AssemblyDiscoveryActionDescription.actionDescription(types.AsEnumerable());

            //assert
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.Equal(2, response.Count());
            Assert.Collection(response,
                (p) => {
                    Assert.Equal(p.BatchName, "BatchFromAttribute");
                    Assert.Equal(p.ActionName, "method1");
                    Assert.Equal(p.ReturnType, typeof(string));
                },
                (p) => {
                    Assert.Equal(p.BatchName, "BatchFromAttribute");
                    Assert.Equal(p.ActionName, "method2");
                    Assert.Equal(p.ReturnType, typeof(int));
                }
            );
        }


        [Fact]
        public void AssemblyDiscoveryActionDescriptionTest_actionDescription_InheritFromBatchAttribute()
        {
            //arrange
            List<TypeInfo> types = new List<TypeInfo>();
            types.Add(typeof(InheritFromBatchAttribute).GetTypeInfo());

            //act
            var response = AssemblyDiscoveryActionDescription.actionDescription(types.AsEnumerable());

            //assert
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.Equal(2, response.Count());
            Assert.Collection(response,
                (p) => {
                    Assert.Equal(p.BatchName, "InheritFromBatchAttribute");
                    Assert.Equal(p.ActionName, "method1");
                    Assert.Equal(p.ReturnType, typeof(string));
                },
                (p) => {
                    Assert.Equal(p.BatchName, "InheritFromBatchAttribute");
                    Assert.Equal(p.ActionName, "method2");
                    Assert.Equal(p.ReturnType, typeof(int));
                }
            );
        }
    }
}
