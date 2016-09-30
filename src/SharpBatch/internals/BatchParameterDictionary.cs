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

using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class BatchParameterDictionary : IDictionary<string, object>
    {
        //to do: check performance to use vector instead of list.
        IList<KeyValuePair<string, object>> _items = new List<KeyValuePair<string, object>>();

        public object this[string key]
        {
            get
            {
                object value = null;
                TryGetValue(key, out value);
                return value;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Count => _items.Count();

        public bool IsReadOnly => true;

        public ICollection<string> Keys => _items.Select(p => p.Key).ToList();

        public ICollection<object> Values => _items.Select(p => p.Value).ToList();

        public void Add(KeyValuePair<string, object> item)
        {
            _items.Add(item);
        }

        public void Add(string key, object value)
        {
            Add(new KeyValuePair<string, object>(key, value));
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return ContainsKey(item.Key);
        }

        public bool ContainsKey(string key)
        {
            return _items.Any(p => p.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            foreach(var item in _items)
            {
                array[arrayIndex++] = item;
            }
            
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _items.Remove(_items.Where(p => p.Key.Equals(item.Key, StringComparison.OrdinalIgnoreCase)).FirstOrDefault());
        }

        public bool Remove(string key)
        {
            return Remove(_items.Where(p => p.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).FirstOrDefault());
        }

        public bool TryGetValue(string key, out object value)
        {
            bool found = false;
            object itemValue;
            try
            {
                itemValue = _items.Where(p => p.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).First().Value;
                found = true;
            }
            catch
            {
                itemValue = null;
                found = false;
            }
            value = itemValue;
            return found;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public bool AddFromQueryString(QueryString queryString)
        {
            return AddFromQueryString(queryString.Value.Substring(1));
        }

        //ToDo manage the right return value
        public bool AddFromQueryString(string queryString)
        {
            if (!string.IsNullOrEmpty(queryString))
            {
                var stringsVector = queryString.Split('&');

                for (var i = 0; i < stringsVector.Length; i++)
                {
                    var itemString = stringsVector[i];
                    var itemVector = itemString.Split('=');

                    Add(itemVector[0], System.Net.WebUtility.UrlDecode(itemVector[1]));
                }
            }
            return true;
        }
    }

}
