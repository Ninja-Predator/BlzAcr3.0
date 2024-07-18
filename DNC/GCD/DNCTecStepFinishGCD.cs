using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Blz.DNC.Data;

namespace Blz.DNC.GCD;

public class TecStepFinishGCD: ISlotResolver
{
    public int Check()
    {
        if (Core.Resolve<JobApi_Dancer>().CompleteSteps == 4 && Core.Resolve<JobApi_Dancer>().IsDancing && Core.Me.GetCurrTarget().CanAttack())
        {
            return 1;
        }
        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.QuadrupleTechnicalFinish.GetSpell());
    }
}