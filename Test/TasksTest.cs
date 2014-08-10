using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TaskUtils
{
    class TasksTest
    {
        [Test]
        public void TestTasks()
        {
            TestTasks((sut, task) => sut.WaitAll(task));
            TestTasks((sut, task) => sut.WaitAll(new Task[] { task },
                TimeSpan.FromSeconds(100)));
        }

        void TestTasks(Action<ITasks, Task> waitAll)
        {
            ITasks sut = new Tasks();
            var done = new ManualResetEvent(false);
            var started = new ManualResetEvent(false);
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var task = sut.StartNew(() =>
            {
                started.Set();
                Assert.AreNotEqual(threadId, Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(TimeSpan.FromSeconds(1));
                done.Set();
            });
            Assert.IsTrue(started.WaitOne(TimeSpan.FromSeconds(0.5)));
            Assert.IsFalse(done.WaitOne(TimeSpan.Zero));
            waitAll(sut, task);
            Assert.IsTrue(done.WaitOne(TimeSpan.Zero));
        }

        [Test, ExpectedException(typeof(TimeoutException))]
        public void TimeoutOccured()
        {
            ITasks sut = new Tasks();
            var task = sut.StartNew(() => Thread.Sleep(TimeSpan.FromSeconds(1)));
            sut.WaitAll(new Task[] { task }, TimeSpan.FromSeconds(0.1));
        }
    }
}
