using System;
using System.Collections.Generic;
using System.Threading;

namespace fair_mark_desktop.Service
{
    public class SchedulerService
    {
        private readonly List<Timer> _timers = new List<Timer>();

        public void ScheduleTask(double intervalInHour, Action task)
        {
            var timer = new Timer(x => { task.Invoke(); }, null, TimeSpan.Zero, TimeSpan.FromHours(intervalInHour));
            _timers.Add(timer);
        }
    }

    public static class WorkSchedulerService
    {
        private static SchedulerService SchedulerService => _schedulerService ?? (_schedulerService = new SchedulerService());
        private static SchedulerService _schedulerService;

        public static void IntervalInHours(double interval, Action task)
        {
            SchedulerService.ScheduleTask(interval, task);
        }
    }
}
