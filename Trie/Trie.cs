using System;
using System.Collections.Generic;
using System.Text;
using TrieImplementation;

namespace Trie
{
    public class Trie<T> : ITrie<T>
    {
        private TrieBase innerTrie;
        private Dictionary<string, HashSet<T>> keyValueObjects;

        #region ITrie<T> Members

        public Trie()
            :this(true)
        {
        }

        public Trie(bool ignoreCase)
        {
            this.innerTrie = new TrieBase(ignoreCase);
            this.keyValueObjects = new Dictionary<string, HashSet<T>>(ignoreCase ?
                StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        }

        public void Insert(T item)
        {
            this.Insert(item.GetHashCode().ToString(), item);
        }

        public void Insert(string key, T value)
        {
            this.InsertToDictionary(key, value);
            this.innerTrie.Insert(key);
        }

        private void InsertToDictionary(string key, T value)
        {
            if (!this.keyValueObjects.ContainsKey(key))
            {
                this.keyValueObjects[key] = new HashSet<T>();
            }

            this.keyValueObjects[key].Add(value);
        }

        public bool Remove(string key)
        {
            if (this.keyValueObjects.ContainsKey(key))
            {
                this.keyValueObjects.Remove(key);
                return true;
            }

            return false;
        }

        public bool Remove(T item)
        {
            return this.Remove(item.GetHashCode().ToString(), item);
        }

        public bool Remove(string key, T item)
        {
            if (this.keyValueObjects.ContainsKey(key))
            {
                return this.keyValueObjects[key].Remove(item);
            }

            return false;
        }

        public void InsertRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                this.Insert(item);
            }
        }

        public bool Contains(T item)
        {
            return this.Contains(item.GetHashCode().ToString());
        }

        public bool Contains(string key)
        {
            return this.innerTrie.Contains(key);
        }

        public ICollection<T> Search(T item)
        {
            return this.Search(item.GetHashCode().ToString(), SearchType.Substring);
        }

        public ICollection<T> Search(string filter, SearchType searchType)
        {
            ICollection<string> strResults = this.innerTrie.Search(filter, searchType);
            ICollection<T> tResults = this.GetValuesFromKeys(strResults);

            return tResults;
        }

        public ICollection<T> FindAll()
        {
            ICollection<string> allKeys = this.innerTrie.FindAll();
            ICollection<T> all = this.GetValuesFromKeys(allKeys);

            return all;
        }
        private ICollection<T> GetValuesFromKeys(ICollection<string> keys)
        {
            List<T> result = new List<T>();
            foreach (string key in keys)
            {
                if (this.keyValueObjects.ContainsKey(key))
                {
                    foreach (T value in this.keyValueObjects[key])
                    {
                        result.Add(value);
                    }
                }
            }

            return result;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            ICollection<T> all = this.FindAll();

            foreach (T item in all)
            {
                yield return item;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
