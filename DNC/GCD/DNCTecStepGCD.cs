using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Blz.DNC;
using Blz.DNC.Data;

namespace Blz.DNC.GCD;

public class DNCTecStepGCD : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.TechnicalStep.IsUnlock())
        {
            return -10;
        }
        if (!DancerRotationEntry.QT.GetQt("爆发"))
        {
            return -5;
        }
        if (!DancerRotationEntry.QT.GetQt("大舞"))
        {
            return -5;
        }
        if (DNCDefinesData.Spells.Devilment.GetSpell().Cooldown.TotalMilliseconds >= 7500.0)
        {
            return -1;
        }
        if (!DNCDefinesData.Spells.TechnicalStep.IsReady())
        {
            return -1;
        }
        if (Core.Me.HasAura(DNCDefinesData.Buffs.StandardStep) || Core.Me.HasAura(DNCDefinesData.Buffs.TechnicalStep))
        {
            return -2;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.TechnicalStep.GetSpell());
    }
}