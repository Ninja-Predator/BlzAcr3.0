using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Blz.DNC;
using Blz.DNC.Data;
using Blz.DNC.Setting;

namespace Blz.DNC.GCD;

public class DNCFinishingMoveGCD : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.FinishingMove.IsUnlock())
        {
            return -10;
        }
        if (!Core.Me.HasAura(DNCDefinesData.Buffs.FinishingMoveReady))
        {
            return -10;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.FinishingMove.GetSpell());
    }
}