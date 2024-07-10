using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Blz.DNC.Data;

namespace Blz.DNC.GCD;

public class DNCStarfallDanceGCD : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.StarfallDance.IsUnlock())
        {
            return -10;
        }
        if (!Core.Me.HasAura(DNCDefinesData.Buffs.FlourishingStarfall))
        {
            return -1;
        }
        return 1;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.StarfallDance.GetSpell());
    }
}