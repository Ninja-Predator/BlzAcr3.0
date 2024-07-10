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

public class DNCFanDance4Ability : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.FanDance4.IsUnlock())
        {
            return -10;
        }
        if (DNCDefinesData.Spells.Devilment.GetSpell().Cooldown.TotalMilliseconds <= 1000.0)
        {
            return -9;
        }
        if (!Core.Me.HasAura(DNCDefinesData.Buffs.FourfoldFanDance) && !DNCDefinesData.Spells.Flourish.RecentlyUsed(1200))
        {
            return -7;
        }
        if (DNCDefinesData.Spells.Devilment.RecentlyUsed(1200))
        {
            return -6;
        }
        if (Core.Resolve<JobApi_Dancer>().IsDancing)
        {
            return -5;
        }
        if (Core.Me.HasAura(DNCDefinesData.Buffs.Devilment))
        {
            return 1;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.FanDance4.GetSpell());
    }
}