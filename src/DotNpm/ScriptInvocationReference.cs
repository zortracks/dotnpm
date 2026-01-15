using System;
using System.Threading.Tasks;

namespace DotNpm {

    public sealed class ScriptInvocationReference {

        public ScriptInvocationReference(Func<Task> invokeFactory) {
            InvokeFactory = invokeFactory;
        }

        public Func<Task> InvokeFactory { get; }
        public TaskStatus Status { get; private set; }

        public Task RunAsync() => InvokeFactory.Invoke().ContinueWith(task => Status = task.Status);
    }
}