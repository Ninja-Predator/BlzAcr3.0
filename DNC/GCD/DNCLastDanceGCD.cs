using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Blz.DNC.Data;

namespace Blz.DNC.GCD;

public class DNCLastDanceGCD : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.LastDance.IsUnlock())
        {
            return -10;
        }
        if (!Core.Me.HasAura(DNCDefinesData.Buffs.LastDanceReady))
        {
            return -10;
        }
        if (DNCDefinesData.Spells.Devilment.GetSpell().Cooldown.TotalSeconds<25.0)
        {
            return -1;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.LastDance.GetSpell());
    }
}