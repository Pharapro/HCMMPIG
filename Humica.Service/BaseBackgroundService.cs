using Quartz;
using System;
using System.Threading.Tasks;

namespace Humica.Service
{
    public abstract class BaseBackgroundService : IJob
    {
        public Type Job { get; set; }
        public string JobName { get; set; }
        public string TriggerName { get; set; }
        public string JobGroup { get; set; }
        public string CronSchedule { get; set; }
        public Task Execute(IJobExecutionContext context)
        {
            DoService();
            return Task.CompletedTask;
        }

        public abstract void DoService();
    }
}
