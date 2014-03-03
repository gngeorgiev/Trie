using System.Collections.Generic;
using TrieImplementation;

namespace Trie
{
    public interface ITrie<T> : IEnumerable<T>
    {
        void Insert(T item);
          
        void InsertRange(IEnumerable<T> items);
          
        bool Contains(T item);

        ICollection<T> Search(T item);

        ICollection<T> Search(string filter, SearchType searchType);

        ICollection<T> FindAll();
    }
}