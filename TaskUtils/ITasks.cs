using System;
using System.Threading.Tasks;

namespace TaskUtils
{
    public interface ITasks
    {
        Task StartNew(Action action);
        void WaitAll(params Task[] tasks);
    }
}
