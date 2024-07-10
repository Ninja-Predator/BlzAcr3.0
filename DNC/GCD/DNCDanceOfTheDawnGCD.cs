using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Blz.DNC.Data;

namespace Blz.DNC.GCD;

public class DNCDanceOfTheDawnGCD : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.DanceOfTheDawn.IsUnlock())
        {
            return -10;
        }
        if (!Core.Me.HasAura(DNCDefinesData.Buffs.DanceOfTheDawnReady))
        {
            return -10;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.DanceOfTheDawn.GetSpell());
    }
}