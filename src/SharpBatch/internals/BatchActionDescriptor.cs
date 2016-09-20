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

namespace SharpBatch.internals
{
    public class BatchActionDescriptor
    {
        private string _id;
        public batchConfigurationDictionary BatchConfiguration { get; } = new batchConfigurationDictionary();

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                BatchConfiguration.Add(BatchConfigurationFieldName.BatchName.ToString(), _id);
            }
        }
        public string BatchName
        {
            get
            {
                return BatchConfiguration[BatchConfigurationFieldName.BatchName.ToString()].ToString();
            }
            set
            {
                BatchConfiguration.AddOrUpdate(BatchConfigurationFieldName.BatchName.ToString(), value);
            }
        }
        public TypeInfo BatchTypeInfo { get; set; }
        public string ActionName
        {
            get
            {
                return BatchConfiguration[BatchConfigurationFieldName.BatchActionName.ToString()].ToString();
            }
            set
            {
                BatchConfiguration.AddOrUpdate(BatchConfigurationFieldName.BatchActionName.ToString(), value);
            }
        }
        public MethodInfo ActionInfo { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public KeyValuePair<string, Type> Parameters { get; set; }

        //Todo
        public IList<object> ExceptionFilter { get; set; }

        public List<IBatchConfigAttributeAsync> ConfigureAttribute { get; set; }

        public List<IBatchExecutionAttribute> ExecutionAttribute { get; set; }

        public Type ReturnType
        {
            get{ return ActionInfo.ReturnType; }
        }

        public bool IsAsync
        {
            get { return typeof(Task).IsAssignableFrom(ReturnType); }
        } 

    }
}
