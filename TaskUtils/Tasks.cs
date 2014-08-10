using System;
using System.Threading.Tasks;

namespace TaskUtils
{
    public class Tasks : ITasks
    {
        public Task StartNew(Action action)
        {
            return Task.Factory.StartNew(action);
        }

        public void WaitAll(params Task[] tasks)
        {
            Task.WaitAll(tasks);
        }

        public void WaitAll(Task[] tasks, TimeSpan timeout)
        {
            var onTime = Task.WaitAll(tasks, timeout);
            if (!onTime)
                throw new TimeoutException();
        }
    }
}
