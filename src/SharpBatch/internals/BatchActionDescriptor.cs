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

        public string Id {
            get { return _id; }
            set
            {
                _id = value;
                BatchConfiguration.Add(BatchConfigurationFieldEnum.BatchName.ToString(), _id);
            }
        }
        public string BatchName { get; set; }
        public TypeInfo BatchTypeInfo { get; set; }
        public string ActionName { get; set; }
        public MethodInfo ActionInfo { get; set; }
        public KeyValuePair<string, Type> Parameters { get; set; }

        public IList<object> ConfigurationFilter { get; set; }
        public IList<object> ExceptionFilter { get; set; }

    }
}
