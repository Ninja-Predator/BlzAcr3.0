using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using Blz.DNC.Data;

namespace Blz.DNC.GCD;

public class DNCStepGCD : ISlotResolver
{
    public int Check()
    {
        if (Core.Resolve<JobApi_Dancer>().IsDancing)
        {
            return 1;
        }
        if (Core.Resolve<JobApi_Dancer>().CompleteSteps == 4)
        {
            return 1;
        }
        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCSpellHelper.GetStep());
    }
}