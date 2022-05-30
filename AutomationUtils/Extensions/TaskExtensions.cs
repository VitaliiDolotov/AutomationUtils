using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutomationUtils.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout, string exceptionDetails)
        {
            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();
                // Very important in order to propagate exceptions
                return await task;
            }

            throw new TimeoutException($"Task failed after {timeout}: {exceptionDetails}");
        }
    }
}
