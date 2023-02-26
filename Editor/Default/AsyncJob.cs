using System;


namespace ch.sycoforge.Unity.Editor
{
    public delegate object AsyncJobRunner(object args);

    public class AsyncJob
    {
        public bool IsReady
        {
            get
            {
                if(time <= 0)
                {
                    return true;
                }

                return time + creationTime <= ConvertToUnixTimestamp(DateTime.Now);
            }
        }

        private AsyncJobRunner runner;
        private object args;
        private float time;
        private float creationTime;


        public AsyncJob(AsyncJobRunner runner, object args) : this(runner, args, -1f)
        {

        }

        public AsyncJob(AsyncJobRunner runner, object args, float time)
        {
            this.runner = runner;
            this.args = args;
            this.time = time;

            creationTime = ConvertToUnixTimestamp(DateTime.Now);
        }

        public virtual object Run()
        {
            return runner(args);
        }

        private static float ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return (float)Math.Floor(diff.TotalSeconds);
        }
    }
}
