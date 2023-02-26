using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor
{
    [InitializeOnLoad]
    public static class AsyncWorker
    {
        private static Queue<AsyncJob> Jobs = new Queue<AsyncJob>();

        static AsyncWorker()
        {
            EditorApplication.update -= EditorUpdate;
            EditorApplication.update += EditorUpdate;
        }

        private static void EditorUpdate()
        {
            AsyncWorker.Process();
        }

        public static int Count
        {
            get
            {
                return Jobs.Count;
            }
        }

        public static void Enqueue(AsyncJob job)
        {
            lock (Jobs)
            {
                Jobs.Enqueue(job);
            }
        }

        public static void Clear()
        {
            lock (Jobs)
            {
                Jobs.Clear();
            }
        }

        private static void Process()
        {
            lock (Jobs)
            {
                while (Jobs.Count > 0)
                {
                    AsyncJob job = Jobs.Peek();

                    if (job != null && job.IsReady)
                    {
                        Jobs.Dequeue();
                        job.Run();
                    }
                }
            }
        }
    }
}
