using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Humica.App_Start
{
    public class BackgroundService
    {
        public void Init_Service(Quartz.IScheduler scheduler)
        {
            Service.BackgroundService.Init_Service(scheduler);
        }
    }
}