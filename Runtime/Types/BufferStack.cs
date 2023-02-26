using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ch.sycoforge.Types
{
    using System;
    using System.Collections;

    [Serializable]
    public class BufferStack<T> : IEnumerable<T>, ICollection, IEnumerable
    {

        //-----------------------------------
        // Properties
        //-----------------------------------
        public int Count
        {
            get { return count; }
        }

        public bool IsSynchronized
        {
            get { return items.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return items.SyncRoot; }
        }

        //-----------------------------------
        // Fields
        //-----------------------------------
        private T[] items;
        private int top;
        private int count;

        //-----------------------------------
        // Constructor
        //-----------------------------------
        public BufferStack(int capacity)
        {
            items = new T[capacity];
        }

        //-----------------------------------
        // Methods
        //-----------------------------------
        public void Push(T item)
        {
            count += 1;
            count = count > items.Length ? items.Length : count;

            items[top] = item;
            top = (top + 1) % items.Length;
        }

        public T Pop()
        {
            count -= 1;
            count = count < 0 ? 0 : count;

            top = (items.Length + top - 1) % items.Length;
            return items[top];
        }

        public T Peek()
        {
            return items[(items.Length + top - 1) % items.Length]; //Same as pop but without changing the value of top.
        }

        public T GetItem(int index)
        {
            if (index > count)
            {
                throw new InvalidOperationException("Index out of bounds");
            }

            else
            {
                //The first element = last element entered = index 0 is at Peek - see above.
                //index 0 = items[(items.Length + top - 1) % items.Length];
                //index 1 = items[(items.Length + top - 2) % items.Length];
                //index 2 = items[(items.Length + top - 3) % items.Length]; etc...
                //So to get an item at a certain index is:
                //items[(items.Length + top - (index+1)) % items.Length];

                return items[(items.Length + top - (index + 1)) % items.Length];
            }
        }

        public void Clear()
        {
            count = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach(T item in items)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            Array.Copy(items, 0, array, index, items.Length);
        }


    }
}
