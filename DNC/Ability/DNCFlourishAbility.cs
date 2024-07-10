using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Blz.DNC;
using Blz.DNC.Data;

namespace Blz.DNC.GCD;

public class DNCFlourishAbility : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.Flourish.IsUnlock())
        {
            return -10;
        }
        if (!DancerRotationEntry.QT.GetQt("爆发"))
        {
            return -5;
        }
        if (!DancerRotationEntry.QT.GetQt("百花"))
        {
            return -5;
        }
        if (DNCDefinesData.Spells.Devilment.GetSpell().Cooldown.TotalMilliseconds <= 1000.0)
        {
            return -1;
        }
        if (!DNCDefinesData.Spells.Flourish.IsReady())
        {
            return -1;
        }
        if (DNCDefinesData.Spells.TechnicalStep.CoolDownInGCDs(2))
        {
            return -4;
        }
        if (Core.Resolve<JobApi_Dancer>().IsDancing)
        {
            return -3;
        }
        if (Core.Me.HasAura(DNCDefinesData.Buffs.TechnicalStep))
        {
            return -6;
        }
        if (DNCDefinesData.Spells.FanDance.RecentlyUsed(1200))
        {
            return -2;
        }
        if (Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.ThreeFoldFanDance) && !DNCDefinesData.Spells.FanDance3.RecentlyUsed(1200))
        {
            return -2;
        }
        if (DNCDefinesData.Spells.Devilment.RecentlyUsed(1200))
        {
            return -4;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.Flourish.GetSpell());
    }
}