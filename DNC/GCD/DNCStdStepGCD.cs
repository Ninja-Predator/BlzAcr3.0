using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Blz.DNC;
using Blz.DNC.Data;

namespace Blz.DNC.GCD;

public class DNCStdStepGCD : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.StandardStep.IsUnlock() && Core.Me.Level < 15U)
        {
            return -10;
        }
        if (!DancerRotationEntry.QT.GetQt("小舞"))
        {
            return -5;
        }
        if (!DNCDefinesData.Spells.StandardStep.IsReady())
        {
            return -1;
        }
        if (AI.Instance.BattleData.CurrBattleTimeInMs <= 30000L && Core.Me.HasAura(DNCDefinesData.Buffs.TechnicalFinish))
        {
            return 0;
        }
        if (Core.Me.HasAura(49U,5000) && DancerRotationEntry.QT.GetQt("爆发药"))
        {
            return -1;
        }
        if (Core.Me.HasAura(DNCDefinesData.Buffs.StandardStep) || Core.Me.HasAura(DNCDefinesData.Buffs.TechnicalStep))
        {
            return -2;
        }
        if (DNCDefinesData.Spells.Flourish.IsUnlock() && DNCDefinesData.Spells.Flourish.GetSpell().Cooldown < TimeSpan.FromSeconds(5.0) && DancerRotationEntry.QT.GetQt("百花"))
        {
            return -3;
        }
        if (DNCDefinesData.Spells.TechnicalStep.IsUnlock() && DNCDefinesData.Spells.Devilment.GetSpell().Cooldown.TotalMilliseconds <= 12000.0 && DancerRotationEntry.QT.GetQt("爆发") && DancerRotationEntry.QT.GetQt("大舞"))
        {
            return -3;
        }
        if (Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.TechnicalFinish))
        {
            return -1;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.StandardStep.GetSpell());
    }
}