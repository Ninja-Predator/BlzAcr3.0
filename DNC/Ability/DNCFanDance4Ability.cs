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
            return -1;
        }
        if (DNCDefinesData.Spells.TechnicalStep.GetSpell().Cooldown.TotalMilliseconds <= 18000.0 && DancerRotationEntry.QT.GetQt("大舞") && DancerRotationEntry.QT.GetQt("爆发"))
        {
            return -1;
        }
        if (!Core.Me.HasAura(DNCDefinesData.Buffs.FourfoldFanDance) && !DNCDefinesData.Spells.Flourish.RecentlyUsed(1200))
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
        if (Core.Me.HasAura(DNCDefinesData.Buffs.Devilment))
        {
            return 1;
        }
        if (!Core.Me.HasAura(DNCDefinesData.Buffs.Devilment))
        {
            double doubletime = DNCDefinesData.Spells.Devilment.GetSpell().Cooldown.TotalMilliseconds + (double)((Core.Resolve<MemApiSpell>().GetGCDDuration()- Core.Resolve<MemApiSpell>().GetElapsedGCD()) * 2);
            int time = (int)doubletime;
            if (Core.Me.HasMyAuraWithTimeleft(DNCDefinesData.Buffs.FourfoldFanDance, time) || DNCDefinesData.Spells.Flourish.RecentlyUsed(1200))
            {
                return -2;
            }
        }
        if (Core.Me.HasAura(DNCDefinesData.Buffs.TechnicalStep))
        {
            return -6;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.FanDance4.GetSpell());
    }
}