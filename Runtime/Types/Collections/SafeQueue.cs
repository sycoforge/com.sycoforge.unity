using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ch.sycoforge.Util.Collections
{
    /// <summary>
    /// Implements a thread safe queue. This queue allows concurrent
    /// access to the queued data.
    /// </summary>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    public class SafeQueue<T>
    {
        //------------------------------------------------
        // Properties
        //------------------------------------------------
        public bool IsEmpty
        {
            get
            {
                return length == 0;
            }
        }

        public int MaxSize
        {
            get
            {
                return maxSize;
            }
        }

        public int Size
        {
            get
            {
                return queue.Count;
            }
        }

        //------------------------------------------------
        // Fields
        //------------------------------------------------
        private readonly Queue<T> queue;
        private readonly int maxSize;
        private int length = 0;

        //------------------------------------------------
        // Constructor
        //------------------------------------------------
        public SafeQueue(int maxSize) 
        { 
            this.maxSize = maxSize;
            this.queue = new Queue<T>();
        }

        //------------------------------------------------
        // Methods
        //------------------------------------------------
        public void Enqueue(T item)
        {
            lock (queue)
            {
                while (queue.Count >= maxSize)
                {
                    Monitor.Wait(queue);
                }
                queue.Enqueue(item);
                length++;
                if (queue.Count == 1)
                {
                    // wake up any blocked dequeue
                    Monitor.PulseAll(queue);
                }
            }
        }
        public T Dequeue()
        {
            lock (queue)
            {
                while (queue.Count == 0)
                {
                    Monitor.Wait(queue);
                }
                T item = queue.Dequeue();
                length = length > 0 ? length - 1 : 0;

                if (queue.Count == maxSize - 1)
                {
                    // wake up any blocked enqueue
                    Monitor.PulseAll(queue);
                }
                return item;
            }
        }

        bool closing;
        public void Close()
        {
            lock (queue)
            {
                closing = true;
                Monitor.PulseAll(queue);
            }
        }
        public bool TryDequeue(out T value)
        {
            lock (queue)
            {
                while (queue.Count == 0)
                {
                    if (closing)
                    {
                        value = default(T);
                        return false;
                    }
                    Monitor.Wait(queue);
                }

                value = queue.Dequeue();
                length = length > 0 ? length - 1 : 0;

                if (queue.Count == maxSize - 1)
                {
                    // wake up any blocked enqueue
                    Monitor.PulseAll(queue);
                }
                return true;
            }
        }
    }
}
