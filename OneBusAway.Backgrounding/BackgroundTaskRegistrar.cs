/* Copyright 2013 Microsoft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
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
