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

public class DNCFanDanceAbility : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.FanDance.IsUnlock())
        {
            return -10;
        }
        if (DancerRotationEntry.QT.GetQt("燃尽爆发") && Core.Resolve<JobApi_Dancer>().FourFoldFeathers >= 1)
        {
            return 0;
        }
        if (DNCDefinesData.Spells.Devilment.GetSpell().Cooldown.TotalMilliseconds <= 1000.0)
        {
            return -1;
        }
        if (Core.Resolve<JobApi_Dancer>().FourFoldFeathers < 1)
        {
            return -1;
        }
        if (Core.Resolve<JobApi_Dancer>().FourFoldFeathers > 3 && (Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.FlourishingSymmetry) || Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.FlourshingFlow) || Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.SilkenFlow) || Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.SilkenSymmetry)))
        {
            return 0;
        }
        if (Core.Resolve<JobApi_Dancer>().IsDancing)
        {
            return -3;
        }
        if (DNCDefinesData.Spells.FanDance.RecentlyUsed(1200))
        {
            return -2;
        }
        if (Core.Me.HasAura(DNCDefinesData.Buffs.TechnicalStep))
        {
            return -6;
        }
        if (Core.Me.HasAura(DNCDefinesData.Buffs.ThreeFoldFanDance) && !DNCDefinesData.Spells.FanDance3.RecentlyUsed(1200))
        {
            return -3;
        }
        if (DNCDefinesData.Spells.Flourish.RecentlyUsed(1200))
        {
            return -4;
        }
        if (Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.Devilment))
        {
            return 1;
        }
        if (Core.Resolve<JobApi_Dancer>().FourFoldFeathers > 3 && (Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.FlourishingSymmetry) || Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.FlourshingFlow) || Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.SilkenFlow) || Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.SilkenSymmetry)))
        {
            return 0;
        }
        return -4;
    }

    public void Build(Slot slot)
    {
        Spell spell = DNCDefinesData.Spells.FanDance.GetSpell();
        if (DNCDefinesData.Spells.FanDance2.IsUnlock() && TargetHelper.CheckNeedUseAoeByMe(5, 5, 2) && DancerRotationEntry.QT.GetQt("AOE"))
        {
            spell = DNCDefinesData.Spells.FanDance2.GetSpell();
        }
        slot.Add(spell);
    }
}