using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ch.sycoforge.Util.Collections
{
    public class RingBuffer<T> //: IList<T>
    {
        //------------------------------------
        // Properties
        //------------------------------------

        public int Capacity
        {
            get
            {
                return capacity;
            }

            set
            {
                capacity = value;
            }
        }

        public int Count
        {
            get
            {
                return ringIndex;
            }
        }

        public bool IsReadOnly
        {
            get;
            set;
        }

        public T this[int index]
        {
            get
            {
                // validate the index
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException();
                }


                return ringBuffer[index];
            }
            set { Insert(index, value); }
        }

        //------------------------------------
        // Static Fields
        //------------------------------------

        //------------------------------------
        // Fields
        //------------------------------------
        private int capacity;
        private int ringIndex;
        private int version = 0;

        private T[] ringBuffer;

        //------------------------------------
        // Constructor
        //------------------------------------
        public RingBuffer(int capacity)
        {
            this.capacity = capacity;

            // validate capacity
            if (capacity <= 0)
            {
                throw new ArgumentException("Initial capacity has to be greater than zero.", "capacity");
            }

            ringBuffer = new T[capacity];
        }

        //------------------------------------
        // ICollection Methods
        //------------------------------------

        //public void GetEnumerator(out T t)
        //{
        //    t = null;
        //    //return null;
        //}

        public T First()
        {
            return ringBuffer[0];
        }

        public T Last()
        {
            return ringBuffer[(ringIndex - 1) % capacity];
        }

        public void Add(T item)
        {
            ringBuffer[ringIndex++] = item;

            ringIndex %= capacity;

            version++;
        }

        public void Clear()
        {
            for (int i = 0; i < capacity; i++)
            {
                ringBuffer[i] = default(T);
            }

            version++;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < capacity; i++)
            {
                if (ringBuffer[i].Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        public void Insert(int index, T item)
        {
            // validate index
            if (index >= capacity)
            {
                throw new IndexOutOfRangeException();
            }

            ringBuffer[index] = item;

            version++;
        }

        /// <summary>
        /// Copies the current items within the buffer to a specified array.
        /// </summary>
        /// <param name="array">The target array to copy the items of 
        /// the buffer to.</param>
        /// <param name="arrayIndex">The start position witihn the target
        /// array to start copying.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < Count; i++)
            {
                array[i + arrayIndex] = ringBuffer[(ringIndex - Count + i) % Capacity];
            }
        }

        /// <summary>
        /// Gets an enumerator over the current items within the buffer.
        /// </summary>
        /// <returns>An enumerator over the current items within the buffer.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            long tmpVersion = version;
            for (int i = 0; i < Count; i++)
            {
                if (tmpVersion != version)
                {
                    throw new InvalidOperationException("Collection changed");
                }

                yield return this[i];
            }
        }

        /// <summary>
        /// Removes an item at a specified position within the buffer.
        /// </summary>
        /// <param name="index">The position of the item to be removed.</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <remarks>
        /// <b>Warning</b>
        /// Frequent usage of this method might become a bad idea if you are 
        /// working with a large buffer capacity. The deletion requires a move 
        /// of all items stored abouve the found position.
        /// </remarks>
        public void RemoveAt(int index)
        {
            // validate the index
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException();
            // move all items above the specified position one step
            // closer to zeri
            for (int i = index; i < Count - 1; i++)
            {
                // get the next relative target position of the item
                int to = (ringIndex - Count + i) % Capacity;
                // get the next relative source position of the item
                int from = (ringIndex - Count + i + 1) % Capacity;
                // move the item
                ringBuffer[to] = ringBuffer[from];
            }
            // get the relative position of the last item, which becomes empty
            // after deletion and set the item as empty
            int last = (ringIndex - 1) % Capacity;
            ringBuffer[last] = default(T);
            // adjust storage information
            ringIndex--;
            //Count--;
            // buffer changed; next version
            version++;
        }

    }
}
