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

public class DNCProcsGCD : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.ReverseCascade.IsUnlock())
        {
            return -10;
        }
        if (!Core.Me.HasAura(DNCDefinesData.Buffs.FlourishingSymmetry) && !Core.Me.HasAura(DNCDefinesData.Buffs.FlourshingFlow) && !Core.Me.HasAura(DNCDefinesData.Buffs.SilkenFlow) && !Core.Me.HasAura(DNCDefinesData.Buffs.SilkenSymmetry))
        {
            return -1;
        }
        if (Core.Resolve<JobApi_Dancer>().FourFoldFeathers == 4)
        {
            return -2;
        }
        if (DNCDefinesData.Spells.TechnicalStep.GetSpell().Cooldown.TotalMilliseconds <= 500.0 && DNCDefinesData.Spells.Devilment.GetSpell().Cooldown.TotalMilliseconds <= 7500.0 && DancerRotationEntry.QT.GetQt("爆发") && DancerRotationEntry.QT.GetQt("大舞"))
        {
            return -1;
        }
        if (DancerRotationEntry.QT.GetQt("燃尽爆发"))
        {
            if (!Core.Me.HasAura(DNCDefinesData.Buffs.FlourishingSymmetry) && !Core.Me.HasAura(DNCDefinesData.Buffs.FlourshingFlow) && !Core.Me.HasAura(DNCDefinesData.Buffs.SilkenFlow))
            {
                Core.Me.HasAura(DNCDefinesData.Buffs.SilkenSymmetry);
            }
            return 0;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCSpellHelper.GetProcGCDCombo());
    }
}