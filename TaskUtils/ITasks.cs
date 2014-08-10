using System;
using System.Threading.Tasks;

namespace TaskUtils
{
    public interface ITasks
    {
        Task StartNew(Action action);
        void WaitAll(params Task[] tasks);
        void WaitAll(Task[] tasks, TimeSpan timeout);
    }
}
