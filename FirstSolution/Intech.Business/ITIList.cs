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

        public void Add( T value )
        {
            if( _array.Length == _count )
            {
                var newOne = new T[ _array.Length * 2 ];
                Array.Copy( _array, newOne, _array.Length );
                _array = newOne;
            }
            _array[_count++] = value;
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

        public void RemoveAt( int i )
        {
            if( i < 0 || i >= _count ) throw new IndexOutOfRangeException();
            Array.Copy( _array, i + 1, _array, i, _count - (i+1) );
            _array[--_count] = default( T );
        }

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

        public int IndexOf(T element)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_array[i].Equals(element))
                    return i;
            }
            return -1;
        }

        //This is a nested type
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
                //nothing to do
            }

            //implement old version of current
            object IEnumerator.Current
            {
                get 
                {
                    return Current;
                }
            }

            public bool MoveNext()
            {
                if (++_currentIndex >= _list.Count)
                    return false;
                return true;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new E(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
