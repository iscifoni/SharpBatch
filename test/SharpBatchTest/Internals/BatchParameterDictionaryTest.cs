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
using SharpBatch;
using SharpBatch.internals;
using Xunit;


namespace SharpBatchTest.Internals
{
    public class BatchParameterDictionaryTest
    {
        [Fact]
        public void BatchParameterDictionaryTest_Count()
        {
            //Arrange
            var batchParameterDictionary = new BatchParameterDictionary();

            //Act
            batchParameterDictionary.Add("key", "value");
            batchParameterDictionary.Add("key1", "value");
            batchParameterDictionary.Add("key2", "value");
            batchParameterDictionary.Add("key3", "value");

            //Assert

            Assert.Equal(4, batchParameterDictionary.Count);

        }

        [Fact]
        public void BatchParameterDictionaryTest_AddKeyValue()
        {
            //Arrange
            var batchParameterDictionary = new BatchParameterDictionary();

            //Act
            batchParameterDictionary.Add("key", "value");

            //Assert
            Assert.Equal(1, batchParameterDictionary.Count);
        }

        [Fact]
        public void BatchParameterDictionaryTest_AddValueKeyPair()
        {
            //Arrange
            var batchParameterDictionary = new BatchParameterDictionary();

            //Act
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key", "value"));

            //Assert
            Assert.Equal(1, batchParameterDictionary.Count);
        }

        [Fact]
        public void BatchParameterDictionaryTest_RemoveByKey()
        {
            //Arrange
            var batchParameterDictionary = new BatchParameterDictionary();
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key1", "value"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key2", "value"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key3", "value"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key4", "value"));

            //Act
            batchParameterDictionary.Remove("key2");

            //Assert
            Assert.Equal(3, batchParameterDictionary.Count);
            Assert.Collection<KeyValuePair<string, object>>(batchParameterDictionary, 
                (s) => { Assert.Equal("key1", s.Key); },
                (s) => { Assert.Equal("key3", s.Key); },
                (s) => { Assert.Equal("key4", s.Key); }
            );
        }

        [Fact]
        public void BatchParameterDictionaryTest_CheckKeysAndValues()
        {
            //Arrange
            var batchParameterDictionary = new BatchParameterDictionary();
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key1", "value1"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key2", "value2"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key3", "value3"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key4", "value4"));

            //Act
            var keys = batchParameterDictionary.Keys;
            var values = batchParameterDictionary.Values;
            //Assert
            Assert.Collection(keys,
                (s) => { Assert.Equal("key1", s); },
                (s) => { Assert.Equal("key2", s); },
                (s) => { Assert.Equal("key3", s); },
                (s) => { Assert.Equal("key4", s); }
            );

            Assert.Collection(values,
                (s) => { Assert.Equal("value1", s); },
                (s) => { Assert.Equal("value2", s); },
                (s) => { Assert.Equal("value3", s); },
                (s) => { Assert.Equal("value4", s); }
            );
        }

        [Fact]
        public void BatchParameterDictionaryTest_CheckContains()
        {
            //Arrange
            var batchParameterDictionary = new BatchParameterDictionary();
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key1", "value1"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key2", "value2"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key3", "value3"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key4", "value4"));

            //Act
            var responseKey1 = batchParameterDictionary.Contains(new KeyValuePair<string, object>("key1", null));
            var responseKey123 = batchParameterDictionary.Contains(new KeyValuePair<string, object>("key123", null));

            //Assert
            Assert.True(responseKey1);
            Assert.False(responseKey123);
        }

        [Fact]
        public void BatchParameterDictionaryTest_CheckContainsKey()
        {
            //Arrange
            var batchParameterDictionary = new BatchParameterDictionary();
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key1", "value1"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key2", "value2"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key3", "value3"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key4", "value4"));

            //Act
            var responseKey1 = batchParameterDictionary.ContainsKey("key1");
            var responseKey123 = batchParameterDictionary.ContainsKey("key123");

            //Assert
            Assert.True(responseKey1);
            Assert.False(responseKey123);
        }

        [Fact]
        public void BatchParameterDictionaryTest_TryGetValue()
        {
            //Arrange
            var batchParameterDictionary = new BatchParameterDictionary();
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key1", "value1"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key2", "value2"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key3", "value3"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key4", "value4"));

            //Act
            object item;
            var result = batchParameterDictionary.TryGetValue("key2", out item);

            //Assert
            Assert.True(result);
            Assert.NotNull(item);
            Assert.IsType<string>(item);
            Assert.Equal("value2", item);
        }

        [Fact]
        public void BatchParameterDictionaryTest_AddFromQueryStringQueryString()
        {
            //Arrange
            var queryStringValue = new QueryString("?key1=value1&key2=value2&key3=value3");
            var batchParameterDictionary = new BatchParameterDictionary();

            //Act
            batchParameterDictionary.AddFromQueryString(queryStringValue);

            //Assert
            Assert.Collection(batchParameterDictionary,
                (s) =>
                {
                    Assert.Equal("key1", s.Key);
                    Assert.Equal("value1", s.Value);
                },
                (s) =>
                {
                    Assert.Equal("key2", s.Key);
                    Assert.Equal("value2", s.Value);
                },
                (s) =>
                {
                    Assert.Equal("key3", s.Key);
                    Assert.Equal("value3", s.Value);
                });
        }


        [Fact]
        public void BatchParameterDictionaryTest_AddFromQueryStringString()
        {
            //Arrange
            var queryStringValue = "?key1=value1&key2=value2&key3=value3";
            var batchParameterDictionary = new BatchParameterDictionary();

            //Act
            batchParameterDictionary.AddFromQueryString(queryStringValue);

            //Assert
            Assert.Collection(batchParameterDictionary,
                (s) =>
                {
                    Assert.Equal("key1", s.Key);
                    Assert.Equal("value1", s.Value);
                },
                (s) =>
                {
                    Assert.Equal("key2", s.Key);
                    Assert.Equal("value2", s.Value);
                },
                (s) =>
                {
                    Assert.Equal("key3", s.Key);
                    Assert.Equal("value3", s.Value);
                });
        }

        [Fact]
        public void BatchParameterDictionaryTest_Enumerator()
        {
            //Arrange
            var batchParameterDictionary = new BatchParameterDictionary();
            
            //Act
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key1", "value1"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key2", "value2"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key3", "value3"));
            batchParameterDictionary.Add(new KeyValuePair<string, object>("key4", "value4"));

            //Assert
            foreach(var item in batchParameterDictionary)
            {
                Assert.IsType<KeyValuePair<string, object>>(item);
            }
        }
    }
}
