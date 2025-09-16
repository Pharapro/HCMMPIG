using Quartz;
using System;
using System.Linq;
using System.Reflection;

namespace Humica.Service
{
    public static class BackgroundService
    {

        public static void Init_Service(Quartz.IScheduler scheduler)
        {

            var routeConfigs = AppDomain.CurrentDomain.GetAssemblies()
                  .SelectMany(assembly =>
                  {
                      try
                      {
                          return assembly.GetTypes();
                      }
                      catch (ReflectionTypeLoadException ex)
                      {
                          return ex.Types.Where(t => t != null);
                      }
                  })
                  .Where(type => typeof(BaseBackgroundService).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                  .Select(type => (BaseBackgroundService)Activator.CreateInstance(type));

            // Register routes from all found configurations
            foreach (var routeConfig in routeConfigs)
            {
                ITrigger cronTrigger = TriggerBuilder.Create()
                .WithIdentity(routeConfig.TriggerName, routeConfig.JobGroup)
                .WithCronSchedule(routeConfig.CronSchedule) // Runs every 5 minutes
                .Build();
                IJobDetail jobTwo = JobBuilder.Create(routeConfig.Job)
               .WithIdentity(routeConfig.JobName, routeConfig.JobGroup)
               .Build();
                scheduler.ScheduleJob(jobTwo, cronTrigger);
            }


        }
    }
}
