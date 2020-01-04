﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarWolfWorks.Utility
{
    public class WWWStack<T> : IEquatable<List<T>>, IEnumerable<T>
    {
        private List<T> items = new List<T>();
        public int Count => items.Count;
        public void Push(T item)
        {
            items.Add(item);
        }
        public T Pop()
        {
            if (items.Count > 0)
            {
                T temp = items[items.Count - 1];
                items.RemoveAt(items.Count - 1);
                return temp;
            }
            else
                return default;
        }
        public T Peek() => items.Last();
        public T Lift() => items.First();

        public void Remove(Predicate<T> when)
        {
            items.Remove(items.Find(when));
        }

        public void Remove(T item) => items.Remove(item);

        public bool Equals(List<T> compared) => items == compared;

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }
    }
}
