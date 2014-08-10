using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TaskUtils
{
    class SyncTasksTest
    {
        [Test]
        public void TestSyncTasks()
        {
            TestSyncTasks((sut, task) => sut.WaitAll(task));
            TestSyncTasks((sut, task) => sut.WaitAll(new Task[] { task },
                TimeSpan.FromSeconds(100)));
        }

        void TestSyncTasks(Action<ITasks, Task> waitAll)
        {
            ITasks sut = new SyncTasks();
            var done = false;
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var task = sut.StartNew(() =>
            {
                Assert.AreEqual(threadId, Thread.CurrentThread.ManagedThreadId);
                done = true;
            });
            Assert.IsFalse(done);
            waitAll(sut, task);
            Assert.IsTrue(done);
        }

        [Test]
        public void InvokingWaitAllMultipleTimesDoesntInvokeActionMultipleTimes()
        {
            ITasks sut = new SyncTasks();
            var counter = 0;
            var task = sut.StartNew(() => { counter++; });
            sut.WaitAll(task);
            sut.WaitAll(task);
            sut.WaitAll(task, task);
            Assert.AreEqual(1, counter);
        }

        [Test]
        public void TimeoutNeverOccurs()
        {
            ITasks sut = new SyncTasks();
            var task = sut.StartNew(() => Thread.Sleep(TimeSpan.FromSeconds(1)));
            sut.WaitAll(new Task[] { task }, TimeSpan.FromSeconds(0.1));
        }
    }
}
