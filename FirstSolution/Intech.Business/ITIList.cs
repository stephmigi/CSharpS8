using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{ 
    public class ITIList<T> : IEnumerable<T>
    {
        T[] _array;
        int _count;

        public ITIList()
        {
            _array = new T[8];
        }

        public int Count
        {
            get { return _count; }
        }

        public T this[int i]
        {
            get 
            {
                if( i >= _count ) throw new IndexOutOfRangeException();
                return _array[i]; 
            }
            set 
            {
                if( i >= _count ) throw new IndexOutOfRangeException();
                _array[i] = value;
            }
        }

        /// <summary>
        /// Returns the index the first occurence
        /// of a given element
        /// </summary>
        /// <param name="element">The element to look for</param>
        /// <returns>The index of element if found, else -1</returns>
        public int IndexOf(T element)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_array[i].Equals(element))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Add an element to the end of list
        /// </summary>
        /// <param name="value">Element to add</param>
        public void Add(T value)
        {
            if (_array.Length == _count)
            {
                var newOne = new T[_array.Length * 2];
                Array.Copy(_array, newOne, _array.Length);
                _array = newOne;
            }
            _array[_count++] = value;
        }

        // todo : better way to implement insertAt?
        /// <summary>
        /// Insert an element at a given index
        /// </summary>
        /// <param name="index">The index at which element must be added</param>
        /// <param name="value">The element to insert</param>
        public void InsertAt(int index, T value)
        {
            if (index < 0 || index > _count) throw new IndexOutOfRangeException();

            var newOne = _array.Length == _count ? new T[_array.Length * 2] : new T[_array.Length];
            Array.Copy(_array, 0, newOne, 0, index);
            newOne[index] = value;
            Array.Copy(_array, index, newOne, index + 1, _count - index);
            _array = newOne;
            _count++;
        }

        /// <summary>
        /// Removes the elemnt at given index
        /// </summary>
        /// <param name="index">The index of the element to delete</param>
        public void RemoveAt( int index )
        {
            if (index < 0 || index >= _count) throw new IndexOutOfRangeException();
            Array.Copy(_array, index + 1, _array, index, _count - (index + 1));
            _array[--_count] = default( T );
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new E(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // This is a nested type
        // Custom enumerator implementation
        class E : IEnumerator<T>
        {
            readonly ITIList<T> _list;
            int _currentIndex;

            public E(ITIList<T> list)
            {
                _list = list;
                _currentIndex = -1;
            }

            public T Current
            {
                get
                {
                    // use _list._array instead of list because  
                    // this is an implementation 
                    if (_currentIndex < 0)
                        throw new InvalidOperationException("Movenext must be called first");

                    if (_currentIndex >= _list._count)
                        throw new InvalidOperationException("Can't call current if Movenext returned false");
                    return _list._array[_currentIndex];
                }
            }

            public void Dispose()
            {
                // nothing to dispose in this case
                // so don't do anything
            }

            // implement old version of current
            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            /// <summary>
            /// Move to next element
            /// </summary>
            /// <returns>True if it's possible to go to next, else false</returns>
            public bool MoveNext()
            {
                if (++_currentIndex >= _list.Count)
                    return false;
                return true;
            }

            /// <summary>
            /// Useless to implement this
            /// </summary>
            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

    }
}
