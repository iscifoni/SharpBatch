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
        public KeyValuePair<string, Type> Parameters { get; set; }

        //Todo
        public IList<object> ExceptionFilter { get; set; }

        public List<IBatchConfigAttributeAsync> ConfigureAttribute { get; set; } 
    }
}
