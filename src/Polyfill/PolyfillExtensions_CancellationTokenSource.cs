// <auto-generated />

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Link = System.ComponentModel.DescriptionAttribute;

static partial class PolyfillExtensions
{
#if !NET8_0_OR_GREATER

    /// <summary>Communicates a request for cancellation asynchronously.</summary>
    /// <remarks>
    /// <para>
    /// The associated <see cref="CancellationToken" /> will be notified of the cancellation
    /// and will synchronously transition to a state where <see cref="CancellationToken.IsCancellationRequested"/> returns true.
    /// Any callbacks or cancelable operations registered with the <see cref="CancellationToken"/>  will be executed asynchronously,
    /// with the returned <see cref="Task"/> representing their eventual completion.
    /// </para>
    /// <para>
    /// Callbacks registered with the token should not throw exceptions.
    /// However, any such exceptions that are thrown will be aggregated into an <see cref="AggregateException"/>,
    /// such that one callback throwing an exception will not prevent other registered callbacks from being executed.
    /// </para>
    /// <para>
    /// The <see cref="ExecutionContext"/> that was captured when each callback was registered
    /// will be reestablished when the callback is invoked.
    /// </para>
    /// </remarks>
    /// <exception cref="ObjectDisposedException">This <see cref="CancellationTokenSource"/> has been disposed.</exception>
    [Link("https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtokensource.cancelasync")]
    public static Task CancelAsync(this CancellationTokenSource target)
    {
        if (target.IsCancellationRequested)
        {
            // If cancellation has already been requested then we can just return immediately
            return Task.CompletedTask;
        }
        else
        {
            // Run sync Cancel call in Task to avoid possible deadlock.
            // As an example, the CancellationTokenSource_CancelAsync_CallbacksInvokedAsynchronously test
            // will hit a deadlock if we try to just call Cancel directly without it being run in a task
            Task task = Task.Run(() => target.Cancel());

            while (!target.IsCancellationRequested)
            {
                // Don't return until we know that the cancellation request has started, to match the
                // state that the real implemenation for CancelAsync would be in after being called
            }

            return task;
        }
    }

#endif
}
