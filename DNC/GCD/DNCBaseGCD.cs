using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Blz.DNC.Data;

namespace Blz.DNC.GCD;

public class DNCBaseGCD : ISlotResolver
{
    public int Check()
    {
        if (Core.Resolve<MemApiSpell>().GetComboTimeLeft().TotalMilliseconds <= 3000.0)
        {
            return 0;
        }
        if (Core.Me.HasAura(DNCDefinesData.Buffs.StandardStep) || Core.Me.HasAura(DNCDefinesData.Buffs.TechnicalStep))
        {
            return -10;
        }
        if (DNCDefinesData.Spells.TechnicalStep.GetSpell().Cooldown.TotalMilliseconds <= 500.0 && DNCDefinesData.Spells.Devilment.GetSpell().Cooldown.TotalMilliseconds <= 7500.0 && DancerRotationEntry.QT.GetQt("大舞") && DancerRotationEntry.QT.GetQt("爆发"))
        {
            return -1;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCSpellHelper.GetBaseGCDCombo());
    }
}