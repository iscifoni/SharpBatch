﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class batchConfigurationDictionary : IDictionary<string, object>
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
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            return _items.Any(p => p.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
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
                itemValue = _items.Where(p => p.Key.Equals(key)).First().Value;
                found = true;
            }catch
            {
                itemValue = null;
                found = false;
            }
            value = itemValue;
            return found;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool AddOrUpdate(string key, object value)
        {
            bool result = false;
            if ( ContainsKey(key))
            {
                Remove(key);
                result = true;
            }
            Add(key, value);
            return result;
        }
    }
}
