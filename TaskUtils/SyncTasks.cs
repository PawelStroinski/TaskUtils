using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskUtils
{
    public class SyncTasks : ITasks
    {
        readonly IDictionary<Task, Action> taskActions = new Dictionary<Task, Action>();

        public Task StartNew(Action action)
        {
            var task = new Task(() => { });
            taskActions.Add(task, action);
            return task;
        }

        public void WaitAll(params Task[] tasks)
        {
            foreach (var task in tasks)
            {
                var action = taskActions[task];
                if (action != null)
                {
                    action();
                    taskActions[task] = null;
                }
            }
        }
    }
}
