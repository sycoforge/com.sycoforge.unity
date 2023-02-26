using System;
using System.Collections.Generic;
using System.Text;

namespace ch.sycoforge.Util.Collections
{
    public class SafeCyclicQueue<T> : ConcurrentQueue<T>
    {
        int MaxSize = 10;
        public SafeCyclicQueue(int maxSize) : base(maxSize)
        {
            MaxSize = maxSize;
        }

        public new void Enqueue(T obj)
        {
            lock (this)
            {
                while (base.Count > MaxSize - 1)
                {
                    T outObj;
                    outObj = base.Dequeue();
                }
            }

            base.Enqueue(obj);
        }
    }
}
