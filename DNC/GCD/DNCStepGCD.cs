using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Blz.DNC;
using Blz.DNC.Data;
using static Blz.DNC.Data.DNCDefinesData;

namespace Blz.DNC.GCD;

public class DNCStepGCD : ISlotResolver
{
    public int Check()
    {
        if (Core.Resolve<JobApi_Dancer>().IsDancing)
        {
            return 0;
        }
        return -1;
    }

    public void Build(Slot slot)
    {
        Spell spell = DNCSpellHelper.GetStep();
        slot.Add(spell);
        if (spell == DNCDefinesData.Spells.QuadrupleTechnicalFinish.GetSpell() && DNCDefinesData.Spells.Devilment.IsReady())
        {
            slot.Add(DNCDefinesData.Spells.Devilment.GetSpell());
        }
    }
}