TaskUtils
=========
Slimmed down wrapper around [System.Threading.Tasks.Task](http://msdn.microsoft.com/en-us/library/system.threading.tasks.task(v=vs.110).aspx) class with a wrapper implementation for use in running application and a synchronous implementation for use in unit tests so testing simple multithreading code is a piece of cake.

Example
-------


Imagine we want to multithread to improve performance. Original unit test looks like something along the lines of:

	var sut = new Application(new FooStub(), new BarStub());
	var expected = FooStub.value + BarStub.value;
	var actual = sut.Add();
	Assert.AreEqual(expected, actual);

Original implementation is:

	var foo = SlowFoo();
    var bar = SlowBar();
    return foo + bar;
	
We change implementation to invoke `Task.Factory.StartNew` (because we're on .NET 4.0) to multithread:

	var foo = 0;
	var bar = 0;
	var fooTask = Task.Factory.StartNew(() => { foo = SlowFoo(); });
	var barTask = Task.Factory.StartNew(() => { bar = SlowBar(); });
	Task.WaitAll(fooTask, barTask);
	return foo + bar;

This works but maybe unit test needs to be modified to make sure  that `Task.WaitAll` is correctly used? For example, if we change `Task.WaitAll(fooTask, barTask);` to `Task.WaitAll(barTask);` the current unit test will probably still pass but the code will hide a bug.

Don't worry, we don't need to pollute our unit test with threading code. Let's just change implementation to use `TaskUtils`:

	var foo = 0;
	var bar = 0;
	var fooTask = tasks.StartNew(() => { foo = SlowFoo(); });
	var barTask = tasks.StartNew(() => { bar = SlowBar(); });
	tasks.WaitAll(fooTask, barTask);
	return foo + bar;

Here the `tasks` field holds instance of `TaskUtils.ITasks`. This is how we could inject it into application:

	new Application(new Foo(), new Bar(), tasks: new TaskUtils.Tasks());

and in unit test:

	new Application(new FooStub(), new BarStub(), tasks: new TaskUtils.SyncTasks());

Apart from that, unit test doesn't need to change and it will find such errors as mentioned above. Running application will internally invoke the same methods as when we used `System.Threading.Tasks.Task` directly.