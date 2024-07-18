using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Blz.DNC.Data;

namespace Blz.DNC.GCD;

public class DNCLastDanceGCD : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.LastDance.IsUnlock())
        {
            return -10;
        }
        if (!Core.Me.HasAura(DNCDefinesData.Buffs.LastDanceReady))
        {
            return -10;
        }
        if (DNCDefinesData.Spells.Devilment.GetSpell().Cooldown.TotalSeconds<23.0 && DancerRotationEntry.QT.GetQt("爆发") && DancerRotationEntry.QT.GetQt("大舞"))
        {
            return -1;
        }
        if (Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.TechnicalFinish) && Core.Resolve<JobApi_Dancer>().Esprit > 75)
        {
            return -2;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.LastDance.GetSpell());
    }
}