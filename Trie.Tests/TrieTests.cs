using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TrieImplementation;
using System.Diagnostics;

namespace Trie.Tests
{
    [TestClass]
    public class TrieTests
    {
        private List<Dummy> ds = new List<Dummy>();

        private void FillDataSource(int data)
        {
            this.ds.Clear();

            for (int i = 0; i < data; i++)
            {
                ds.Add(new Dummy
                    {
                        Id = i,
                        Name = "Name " + i
                    });
            }
        }

        [TestMethod]
        public void TestWithFiveDummysFindAll()
        {
            this.FillDataSource(5);
            Trie<Dummy> trie = new Trie<Dummy>();

            trie.InsertRange(this.ds);

            var results = trie.FindAll();

            CollectionAssert.AreEqual(results.OrderBy(x => x.Name).ToList(), this.ds);
        }

        [TestMethod]
        public void TestWithFiveDummysFindAllWithContainsFilter()
        {
            this.FillDataSource(5);
            Trie<Dummy> trie = new Trie<Dummy>();

            for (int i = 0; i < 5; i++)
            {
                trie.Insert(this.ds[i].Name, this.ds[i]);
            }

            var results = trie.Search("name", SearchType.Substring);

            CollectionAssert.AreEqual(results.OrderBy(x => x.Name).ToList(), this.ds);
        }

        [TestMethod]
        public void TestWithFiveDummysFindAllWithStartsWithFilter()
        {
            this.FillDataSource(5);
            Trie<Dummy> trie = new Trie<Dummy>();

            foreach (var item in this.ds)
            {
                trie.Insert(item.Name, item);
            }

            var results = trie.Search("name", SearchType.Prefix);

            CollectionAssert.AreEqual(results.ToList(), this.ds);
        }

        [TestMethod]
        public void TestWithFiveDummysFindAllNoKeyInsert()
        {
            this.FillDataSource(5);
            Trie<Dummy> trie = new Trie<Dummy>();

            trie.InsertRange(this.ds);

            var results = trie.FindAll();

            CollectionAssert.AreEqual(results.ToList(), this.ds);
        }

        [TestMethod]
        public void TestForSearchSpeed()
        {
            Trie<Dummy> trie = new Trie<Dummy>();

            List<Dummy> dummys = new List<Dummy>();
            for (int i = 0; i < 1000000; i++)
            {
                Dummy d = new Dummy
                {
                    Id = i,
                    Name = "Name " + i
                };

                dummys.Add(d);
                trie.Insert(d.Name, d);
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            dummys.First(x => x.Name == "Name 99999");
            var elapsedLinear = sw.Elapsed;
            sw.Stop();
            sw.Reset();

            sw.Start();
            trie.Search("Name 99999", SearchType.Prefix);
            var elapsedTrie = sw.Elapsed;
            sw.Stop();

            bool isTrieFaster = elapsedLinear > elapsedTrie;

            Assert.IsTrue(isTrieFaster);
        }
    }
}
