using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace OneBusAway
{
    /// <summary>
    /// Contains useful extension methods for Windows Phone.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// There is no way to get the CoreDispatcher unless we're in native code, 
        /// so we have to make our own RunIdleAsync method for Windows Phone.
        /// </summary>
        public static Task RunIdleAsync(this Dispatcher dispatcher, Action action)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        action();
                    }
                    catch(Exception e)
                    {
                        tcs.SetException(e);
                    }
                    finally
                    {
                        tcs.SetResult(null);
                    }
                });

            return tcs.Task;
        }

        /// <summary>
        /// Overload that returns an actual result.
        /// </summary>
        public static Task<Task<T>> RunIdleAsync<T>(this Dispatcher dispatcher, Func<Task<T>> func)
        {
            TaskCompletionSource<Task<T>> tcs = new TaskCompletionSource<Task<T>>();
            dispatcher.BeginInvoke(() =>
            {
                Task<T> result = Task.FromResult<T>(default(T));
                try
                {
                    result = func();
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
                finally
                {
                    tcs.SetResult(result);
                }
            });

            return tcs.Task;
        }
    }
}
