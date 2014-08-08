using System;
using System.Threading;
using NUnit.Framework;

namespace TaskUtils
{
    class TasksTest
    {
        [Test]
        public void TestTasks()
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
            sut.WaitAll(task);
            Assert.IsTrue(done.WaitOne(TimeSpan.Zero));
        }
    }
}
