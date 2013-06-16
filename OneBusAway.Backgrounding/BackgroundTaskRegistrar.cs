using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OneBusAway.Backgrounding
{
    /// <summary>
    /// This class will attempt to register all of OBA's background tasks.
    /// </summary>
    public sealed class BackgroundTaskRegistrar
    {
        /// <summary>
        /// Try and register all of our background tasks that require lock screen access.
        /// </summary>
        /// <returns></returns>
        public static bool TryRegisterAllBackgroundTasks()
        {
            var backgroundAccessStatus = BackgroundExecutionManager.GetAccessStatus();
            if (backgroundAccessStatus == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                RegisterBackgroundTask(
                    typeof(StartupBackgroundTask),
                    new SystemTrigger(SystemTriggerType.SessionConnected, false),
                    new SystemCondition(SystemConditionType.InternetAvailable));

                RegisterBackgroundTask(
                    typeof(UserPresentBackgroundTask),
                    new SystemTrigger(SystemTriggerType.UserPresent, false),
                    new SystemCondition(SystemConditionType.InternetAvailable));

                RegisterBackgroundTask(
                    typeof(NetworkConnectionDroppedBackgroundTask),
                    new SystemTrigger(SystemTriggerType.NetworkStateChange, false),
                    new SystemCondition(SystemConditionType.InternetNotAvailable));

                RegisterBackgroundTask(
                    typeof(NetworkConnectionEstablishedBackgroundTask),
                    new SystemTrigger(SystemTriggerType.InternetAvailable, false));

                RegisterBackgroundTask(
                    typeof(TimerBackgroundTask),
                    new TimeTrigger(15, false),
                    new SystemCondition(SystemConditionType.InternetAvailable));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Unregisters the background tasks.
        /// </summary>
        public static void UnregisterAllBackgroundTasks(bool cancel)
        {
            UnregisterBackgroundTask(typeof(StartupBackgroundTask), cancel);
            UnregisterBackgroundTask(typeof(UserPresentBackgroundTask), cancel);
            UnregisterBackgroundTask(typeof(NetworkConnectionDroppedBackgroundTask), cancel);
            UnregisterBackgroundTask(typeof(NetworkConnectionEstablishedBackgroundTask), cancel);
            UnregisterBackgroundTask(typeof(TimerBackgroundTask), cancel);
        }

        /// <summary>
        /// Registers a background task if it doesn't already exist.
        /// </summary>
        public static void RegisterBackgroundTask(Type backgroundTaskType, IBackgroundTrigger trigger, params IBackgroundCondition[] conditions)
        {
            bool alreadyRegistered = BackgroundTaskRegistration.AllTasks.Any(kvp => kvp.Value.Name == backgroundTaskType.Name);
            if (!alreadyRegistered)
            {
                BackgroundTaskBuilder sessionStartedTaskBuilder = new BackgroundTaskBuilder();
                sessionStartedTaskBuilder.Name = backgroundTaskType.Name;
                sessionStartedTaskBuilder.TaskEntryPoint = backgroundTaskType.FullName;
                sessionStartedTaskBuilder.SetTrigger(trigger);

                foreach (IBackgroundCondition condition in conditions)
                {
                    sessionStartedTaskBuilder.AddCondition(condition);
                }

                var sessionRegistration = sessionStartedTaskBuilder.Register();
            }
        }

        /// <summary>
        /// Unregisters the background task from Windows.
        /// </summary>
        public static void UnregisterBackgroundTask(Type backgroundTaskType, bool cancel)
        {
            var query = from task in BackgroundTaskRegistration.AllTasks
                        where task.Value.Name == backgroundTaskType.Name
                        select task.Value;

            IBackgroundTaskRegistration registration = query.FirstOrDefault();
            if (registration != null)
            {
                registration.Unregister(cancel);
            }            
        }
    }
}
