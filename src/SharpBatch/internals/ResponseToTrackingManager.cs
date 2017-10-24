using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SharpBatch.Serialization.Abstract;
using SharpBatch.Tracking.Abstraction;

namespace SharpBatch.internals
{
    public class ResponseToTrackingManager
    {
        private ISharpBatchTrackingFactory _sharpBatchTrackingFactory;
        private IModelSerializer _modelSerializer;
        private Guid _sessionId;

        public ResponseToTrackingManager(ISharpBatchTrackingFactory sharpBatchTrackingFactory, IModelSerializer modelSerializer, Guid sessionId)
        {
            _sharpBatchTrackingFactory = sharpBatchTrackingFactory;
            _modelSerializer = modelSerializer;
            _sessionId = sessionId;
        }

        public ResponseToTrackingManager(ISharpBatchTrackingFactory sharpBatchTrackingFactory, Guid sessionId)
        {
            _sharpBatchTrackingFactory = sharpBatchTrackingFactory;
            _sessionId = sessionId;
        }

        /// <summary>
        /// Save the value of content into message section of a <see cref="ISharpBatchTracking"/>
        /// </summary>
        /// <param name="content">The content to save</param>
        /// <returns></returns>
        public Task ToTracking(string content)
        {
            ISharpBatchTracking tracking = _sharpBatchTrackingFactory.getTrakingProvider();
            return tracking.AddMessageAsync(_sessionId, content);
        }

        /// <summary>
        /// Save the serialized value of content into message section of a <see cref="ISharpBatchTracking"/>, 
        /// </summary>
        /// <param name="data">The content to save</param>
        /// <returns></returns>
        /// <remarks>The <see cref="IModelSerializer"/> used is returned from registered service </remarks>
        public Task ToTracking(object data)
        {
            if (_modelSerializer == null)
            {
                throw new ArgumentNullException(nameof(_modelSerializer));
            }

            return ToTracking(_modelSerializer.Serialize(data));
        }

        /// <summary>
        /// Save the serialized value of content into message section of a <see cref="ISharpBatchTracking"/>, 
        /// </summary>
        /// <param name="data">The content to save</param>
        /// <param name="serializer">The <see cref="IModelSerializer"/> to use to serialize data.</param>
        /// <returns></returns>
        public Task ToTracking(object data, IModelSerializer serializer)
        {
            return ToTracking(serializer.Serialize(data));
        }
    }
}
