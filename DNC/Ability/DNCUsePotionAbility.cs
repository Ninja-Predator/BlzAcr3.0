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

public class DNCUsePotionAbility : ISlotResolver
{
    public int Check()
    {
        if (!DancerRotationEntry.QT.GetQt("爆发药"))
        {
            return -1;
        }
        if (Core.Resolve<MemApiSpell>().GetCooldown(DNCDefinesData.Spells.Cascade).TotalMilliseconds < 600)
        {
            return -99;
        }
        if (!DNCDefinesData.Spells.Potion.IsReady())
        {
            return -1;
        }
        if (DNCDefinesData.Spells.TechnicalStep.GetSpell().Cooldown.TotalMilliseconds <= (double)(Core.Resolve<MemApiSpell>().GetGCDDuration() - Core.Resolve<MemApiSpell>().GetElapsedGCD()) && DNCDefinesData.Spells.Devilment.GetSpell().Cooldown.TotalMilliseconds <= (double)(7500 + Core.Resolve<MemApiSpell>().GetGCDDuration()))
        {
            return 0;
        }
        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(Spell.CreatePotion());
        slot.Wait2NextGcd = true;
    }
}