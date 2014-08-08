using System.Threading;
using NUnit.Framework;

namespace TaskUtils
{
    class SyncTasksTest
    {
        [Test]
        public void TestSyncTasks()
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
            sut.WaitAll(task);
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
    }
}
