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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class ShareMessageCollection : IShareMessageCollection
    {
        private KeyValuePair<Type, object>[] items = new KeyValuePair<Type, object>[0];

        public object this[Type key]
        {
            get
            {
                for(var i = 0;i<items.Length;i++)
                {
                    var item = items[i];
                    if (item.Key.Equals(key))
                    {
                        return item.Value;
                    }
                }
                return null;
            }
            set
            {
                KeyValuePair<Type, object>[] newItems = new KeyValuePair<Type, object>[items.Length + 1];
                items.CopyTo(newItems, 0);
                newItems[newItems.Length - 1] = new KeyValuePair<Type, object>(key ,value);
                items = newItems;
            }
        }

        public TMessage Get<TMessage>()
        {
            return (TMessage)this[typeof(TMessage)];
        }

        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
        {
            for(var i = 0; i<items.Length;i++)
            {
                var item = items[i];
                yield return item;
            }
        }

        public bool remove<TMessage>()
        {
            var newItems = new KeyValuePair<Type, object>[items.Length - 1];
            var newIndex = 0;
            var itemFound = false;

            for(var i=0; i<items.Length;i++)
            {
                var item = items[i];
                if (!item.Key.Equals(typeof(TMessage)))
                {
                    newItems[newIndex] = item;
                    newIndex++;
                }
                else
                {
                    itemFound = true;
                }
            }

            return itemFound;
        }

        public void Set<TMessage>(TMessage instance)
        {
            this[typeof(TMessage)] = instance;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
