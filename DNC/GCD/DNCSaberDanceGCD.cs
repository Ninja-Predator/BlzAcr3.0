using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Blz.DNC.Data;
using Blz.DNC.Setting;

namespace Blz.DNC.GCD;

public class DNCSaberDanceGCD : ISlotResolver
{
    public int Check()
    {
        if (Core.Me.HasAura(344U))
        {
            return -1;
        }
        if (!DNCDefinesData.Spells.SaberDance.IsUnlock())
        {
            return -10;
        }
        if (DancerRotationEntry.QT.GetQt("燃尽爆发") && Core.Resolve<JobApi_Dancer>().Esprit >= 50)
        {
            return 0;
        }
        if (!DancerRotationEntry.QT.GetQt("爆发"))
        {
            if (Core.Resolve<JobApi_Dancer>().Esprit >= 100)
            {
                return 1;
            }
            return -5;
        }
        else
        {
            if (DNCDefinesData.Spells.TechnicalStep.GetSpell().Cooldown.TotalMilliseconds <= 500.0 && DNCDefinesData.Spells.Devilment.GetSpell().Cooldown.TotalMilliseconds <= 7500.0 && DancerRotationEntry.QT.GetQt("爆发") && DancerRotationEntry.QT.GetQt("大舞"))
            {
                return -1;
            }
            if (Core.Me.HasAura(DNCDefinesData.Buffs.StandardStep) || Core.Me.HasAura(DNCDefinesData.Buffs.TechnicalStep))
            {
                return -2;
            }
            if (Core.Resolve<JobApi_Dancer>().Esprit < 50)
            {
                return -1;
            }
            if (Core.Me.HasAura(DNCDefinesData.Buffs.TechnicalFinish))
            {
                return 0;
            }
            if (Core.Resolve<JobApi_Dancer>().Esprit >= DNCSettings.Instance.espritThreshold)
            {
                return 1;
            }
            return -4;
        }
    }

    public void Build(Slot slot)
    {
        slot.Add(Core.Resolve<MemApiSpell>().CheckActionChange(DNCDefinesData.Spells.SaberDance).GetSpell());
    }
}