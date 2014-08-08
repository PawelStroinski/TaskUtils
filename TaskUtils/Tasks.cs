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
    }
}
