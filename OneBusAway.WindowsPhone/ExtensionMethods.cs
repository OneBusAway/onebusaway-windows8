using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
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

        /// <summary>
        /// Utility method that returns true if a number is either 0 or NaN.
        /// </summary>
        public static bool IsZeroOrNaN(this double d)
        {
            return d == 0 || double.IsNaN(d);
        }

        /// <summary>
        /// Utility method converts a point to a geocoordinate.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static GeoCoordinate ToCoordinate(this OneBusAway.Model.Point point)
        {
            return new GeoCoordinate(point.Latitude, point.Longitude);
        }
        
        /// <summary>
        /// Allows callers to await the completion of an animation.
        /// </summary>
        public static Task WaitForStoryboardToFinishAsync(this Storyboard storyboard)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            EventHandler onCompleted = null;
            onCompleted = (sender, e) =>
                {
                    storyboard.Completed -= onCompleted;
                    tcs.SetResult(null);
                };

            storyboard.Completed += onCompleted;
            storyboard.Begin();
            return tcs.Task;
        }
    }
}
