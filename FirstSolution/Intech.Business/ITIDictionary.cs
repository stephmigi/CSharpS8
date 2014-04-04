using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class ITIDictionary <TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private int _count;
        Bucket[] _buckets;
        static readonly int[] _primeNumbers = new int[]{ 11, 23, 47, 97, 199, 397, 809 };
        IEqualityComparer<TKey> _strategy;

        private class Bucket
        {
            public readonly TKey Key;
            public TValue Value;
            public Bucket Next;

            public Bucket(TKey key, TValue value, Bucket bucket)
            {
                Key = key;
                Value = value;
                Next = bucket;
            }
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }

        public ITIDictionary()
        {
            _buckets = new Bucket[11];
            _strategy = EqualityComparer<TKey>.Default;
        }

        public ITIDictionary(IEqualityComparer<TKey> strategy)
        {
            if (strategy == null) throw new ArgumentException();
            _buckets = new Bucket[11];
            _strategy = strategy;
        }

        /// <summary>
        /// Removes an element from the dictionnary
        /// </summary>
        /// <param name="key">The key of the element to remove</param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            int slot = Math.Abs(_strategy.GetHashCode(key) % _buckets.Length);
            Bucket b = _buckets[slot];
            if (b == null)
            {
                throw new InvalidOperationException("Key doesn't exist.");
            }
            else
            {
                Bucket previous = null;
                while(b != null)
                {
                    if (_strategy.Equals(b.Key, key))
                    {
                        if (previous != null)
                        {
                            previous.Next = b.Next;
                        }
                        else
                        {
                            _buckets[slot] = b.Next;
                        }
                        b = null;
                        _count--;
                        return true;
                    }
                    previous = b;
                    b = b.Next;
                }
                return false;
            }
        }

        /// <summary>
        /// Adds an element to the dictionnary
        /// </summary>
        /// <param name="key">The key to add</param>
        /// <param name="value">The associated value</param>
        public void Add(TKey key, TValue value)
        {
            AddOrReplace(key, value, true);
        }

        /// <summary>
        /// Tries to get a value from the dictionnary
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <param name="value">The value if found</param>
        /// <returns>True if found else false</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            int hash = _strategy.GetHashCode(key);
            int slot = Math.Abs(hash % _buckets.Length);
            Bucket b = _buckets[slot];

            while (b != null)
            {
                if (_strategy.Equals(b.Key, key))
                {
                    value = b.Value;
                    return true;
                }
                b = b.Next;
            }
            return false;
        }

        /// <summary>
        /// Get a value based on a key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The given value</returns>
        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (!TryGetValue(key, out value))
                    throw new KeyNotFoundException();
                return value;
            }
            set
            {
                AddOrReplace(key, value, false);
            }
        }

        private void AddOrReplace(TKey key, TValue value, bool isAdd)
        {
            int hash = _strategy.GetHashCode(key);
            int slot = Math.Abs(hash % _buckets.Length);
            Bucket b = _buckets[slot];

            if (b == null)
            {
                AddNewBucket(key, value, slot);
            }
            else
            {
                do
                {
                    if (_strategy.Equals(b.Key, key))
                    {
                        if (isAdd)
                            throw new InvalidOperationException("Can't add existing key.");
                        b.Value = value;
                        return;
                    }
                    b = b.Next;
                }
                while (b != null);
                AddNewBucket(key, value, slot);
            }
        }

        private Bucket AddNewBucket(TKey key, TValue value, int slot)
        {
            _count++;

            // new element is added in first position in bucket
            var b = new Bucket(key, value, _buckets[slot]);
            _buckets[slot] = b;

            // if buckets contain more than 3 elements, increase _buckets
            int avgCount = _count / _buckets.Length;
            if (avgCount > 3)
            {
                Grow();
            }

            return b;
        }

        private void Grow()
        {
            int newCapacity = _primeNumbers[Array.IndexOf(_primeNumbers, _buckets.Length) + 1];
            Bucket[] newBuckets = new Bucket[newCapacity];

            for (int i = 0; i < _buckets.Length; i++)
            {
                Bucket b = _buckets[i];
                while (b != null)
                {
                    int newSlot = Math.Abs(_strategy.GetHashCode(b.Key) % newBuckets.Length);
                    var oldNext = b.Next;
                    b.Next = newBuckets[newSlot];
                    newBuckets[newSlot] = b;
                    b = oldNext;
                }
            }
            _buckets = newBuckets;
        }
    }
}
