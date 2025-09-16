using Humica.Core.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Humica.Condition
{
    public class ClsEmployeeCondition
    {
        public Dictionary<string, Action> BuildUpdateMap(string ChangedTo, HRStaffProfile staff)
        {
            return new Dictionary<string, Action>
            {
                { "Head of Department", () => { if (ChangedTo != staff.HODCode) staff.HODCode = ChangedTo; } },
                { "First Line",         () => { if (ChangedTo != staff.FirstLine) staff.FirstLine = ChangedTo; } },
                { "First Line 2",       () => { if (ChangedTo != staff.FirstLine2) staff.FirstLine2 = ChangedTo; } },
                { "Second Line",        () => { if (ChangedTo != staff.SecondLine) staff.SecondLine = ChangedTo; } },
                { "Second Line 2",      () => { if (ChangedTo != staff.SecondLine2) staff.SecondLine2 = ChangedTo; } },
                { "OT First Line",      () => { if (ChangedTo != staff.OTFirstLine) staff.OTFirstLine = ChangedTo; } },
                { "OT Second Line",     () => { if (ChangedTo != staff.OTSecondLine) staff.OTSecondLine = ChangedTo; } },
                { "OT Third Line",      () => { if (ChangedTo != staff.OTthirdLine) staff.OTthirdLine = ChangedTo; } },
                { "Appraisal 1",        () => { if (ChangedTo != staff.APPAppraisal) staff.APPAppraisal = ChangedTo; } },
                { "Appraisal 2",        () => { if (ChangedTo != staff.APPAppraisal2) staff.APPAppraisal2 = ChangedTo; } },
                { "Evaluator",          () => { if (ChangedTo != staff.APPEvaluator) staff.APPEvaluator = ChangedTo; } },
                { "Tracking",           () => { if (ChangedTo != staff.APPTracking) staff.APPTracking = ChangedTo; } }
            };
        }
    }
}
