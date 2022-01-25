using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

    public class WorkSchedulerService
    {
        public static SchedulerService SchedulerService
        {
            get
            {
                if (_schedulerService == null)
                    _schedulerService = new SchedulerService();
                return _schedulerService;
            }
        }
        private static SchedulerService _schedulerService = null;

        public static void IntervalInHours(double interval, Action task)
        {
            SchedulerService.ScheduleTask(interval, task);
        }
    }
}
