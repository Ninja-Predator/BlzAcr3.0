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

public class DNCFanDance3Ability : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.FanDance3.IsUnlock())
        {
            return -10;
        }
        if (DancerRotationEntry.QT.GetQt("燃尽爆发") && Core.Me.HasAura(DNCDefinesData.Buffs.ThreeFoldFanDance))
        {
            return 0;
        }
        if (DNCDefinesData.Spells.Devilment.GetSpell().Cooldown.TotalMilliseconds <= 1000.0)
        {
            return -1;
        }
        if (DNCDefinesData.Spells.Devilment.RecentlyUsed(1200))
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
        if (!Core.Me.HasAura(DNCDefinesData.Buffs.ThreeFoldFanDance))
        {
            return -1;
        }
        if (DNCDefinesData.Spells.Flourish.RecentlyUsed(1200) && !DNCDefinesData.Spells.FanDance3.RecentlyUsed(1200))
        {
            return 1;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.FanDance3.GetSpell());
    }
}