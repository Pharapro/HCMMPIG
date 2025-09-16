using Humica.Employee;
using System;

namespace Humica.Service
{
    public class ClsSVUpdateEmpCarMovement : BaseBackgroundService
    {
        public ClsSVUpdateEmpCarMovement()
        {
            JobName = "SendCareer";
            TriggerName = "SendCareerTrigger";
            JobGroup = "SendCareer";
            //CronSchedule = "0 0/3 * * * ?"; // create a trigger that fires every 5 minutes, at 10 seconds after the minute
            CronSchedule = "0 20 * * * ?"; // Every day at 8:00 AM
            Job = GetType();
        }
        public override void DoService()
        {

            //Update EmpCarer
            ClsEmpCareerMovement careerMovement = new ClsEmpCareerMovement();
            careerMovement.ListUpdateCareer();

        }
    }
}
