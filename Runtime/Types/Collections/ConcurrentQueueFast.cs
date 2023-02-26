using System;
using System.Collections;
using System.Collections.Generic;
#if SERIALIZATION
using System.Runtime.Serialization;
#endif

using System.Security.Permissions;
using System.Text;

namespace ch.sycoforge.Util.Collections
{


    [Serializable]
    public class ConcurrentQueueFast<T> :
            SynchronizedCollection,
            ICollection,
            IEnumerable, IEnumerable<T>
#if SERIALIZATION
            ,ISerializable, IDeserializationCallback
#endif

    {
#if SERIALIZATION
        private SerializationInfo _info;
#endif
        private T[] _items;
        private volatile int _head;
        private volatile int _size;
        private volatile int count = 0;

        public ConcurrentQueueFast()
            : this(0)
        {
        }
        public ConcurrentQueueFast(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");

            this._items = new T[capacity];
        }
        public ConcurrentQueueFast(IEnumerable<T> collection)
            : this()
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            foreach (T item in collection)
                this.Enqueue(item);
        }
#if SERIALIZATION
        protected ConcurrentQueueFast(SerializationInfo info, StreamingContext context)
        {
            this._info = info;
        }
#endif

        public int Count
        {
            get { return this._size; }
        }

        public bool IsEmpty
        {
            get { return this.count == 0; }
        }

        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        object ICollection.SyncRoot
        {
            get { return this; }
        }

        public void Clear()
        {
            lock (this._synclock)
                Array.Clear(this._items, this._head, this._size);
        }

        public bool Contains(T item)
        {
            if (this._size > 0)
            {
                lock (this._synclock)
                    return (Array.IndexOf<T>(this._items, item, this._head, this._size) >= 0);
            }

            return false;
        }

        public void CopyTo(T[] array)
        {
            this.CopyTo(array, 0);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if ((arrayIndex < 0) || (arrayIndex >= array.Length))
                throw new ArgumentOutOfRangeException("arrayIndex");

            if (this._size > 0)
            {
                lock (this._synclock)
                    Array.Copy(this._items, this._head, array, arrayIndex, Math.Min((array.Length - arrayIndex), this._size));
            }
        }

        public T Dequeue()
        {
            if (this._size < 1)
                throw new InvalidOperationException();

            lock (this._synclock)
            {
                T item = this._items[this._head];
                this._items[this._head] = default(T);

                if (++this._head >= this._items.Length)
                    this._head = 0;

                count--;

                return item;
            }
        }

        public void Enqueue(T item)
        {
            lock (this._synclock)
            {
                if (this._size++ > this._items.Length)
                {
                    if (this._head > 0)
                    {
                        Array.Copy(this._items, this._head, this._items, 0, this._size);
                    }
                    else
                    {
                        Array.Resize<T>(ref this._items, ((this._items.Length << 1) | 1));
                    }
                }

                this._items[this._head + (this._size - 1)] = item;

                count++;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (this._synclock)
            {
                for (int i = this._head; i < this._size; i++)
                    yield return this._items[i];
            }
        }

#if  SERIALIZATION
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            lock (this._synclock)
                info.AddValue("items", this.ToArray());
        }

        public virtual void OnDeserialization(object sender)
        {
            lock (this._synclock)
            {
                if (this._info != null)
                {
                    T[] items = ((T[])this._info.GetValue("items", typeof(T[])));
                    this._items = new T[((items.Length << 1) | 1)];
                    this._head = 0;
                    this._size = items.Length;

                    Array.Copy(items, 0, this._items, 0, items.Length);

                    this._info = null;
                }
            }
        }
#endif

        public T Peek()
        {
            if (this._size < 1)
                throw new InvalidOperationException();

            lock (this._synclock)
                return this._items[this._head];
        }

        public T[] ToArray()
        {
            lock (this._synclock)
            {
                T[] array = new T[this._size];

                this.CopyTo(array);
                return array;
            }
        }

        public void TrimExcess()
        {
            lock (this._synclock)
            {
                this._items = this.ToArray();
                this._head = 0;
                this._size = this._items.Length;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if ((index < 0) || (index >= array.Length))
                throw new ArgumentOutOfRangeException("index");

            if (this._size > 0)
            {
                lock (this._synclock)
                    Array.ConstrainedCopy(this._items, this._head, array, index, Math.Min((array.Length - index), this._size));
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}


