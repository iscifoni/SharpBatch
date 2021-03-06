﻿//Copyright 2016 Scifoni Ivano
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
    public class AssemblyDiscoveryManagerTest
    {
        [Fact]
        public void AssemblyDiscoveryManagerTest_enlistDependencies()
        {
            //Arrange
            var currentAssemblyName = typeof(AssemblyDiscoveryManagerTest).GetTypeInfo().Assembly.GetName().Name;
            var assemblies = AssemblyDiscoveryManager.EnlistAssemblyDependencies( currentAssemblyName );

            //Act
            var xunitAssembly = assemblies.Where(p => string.Equals(p.GetName().Name, typeof(FactAttribute).GetTypeInfo().Assembly.GetName().Name ));

            //Assert
            Assert.NotNull(xunitAssembly);

        }
    }
}
