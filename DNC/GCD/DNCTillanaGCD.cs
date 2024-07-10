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

public class DNCTillanaGCD : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.Tillana.IsUnlock())
        {
            return -10;
        }
        if (!Core.Me.HasAura(DNCDefinesData.Buffs.FlourishingFinish))
        {
            return -10;
        }
        if (!Core.Me.HasMyAuraWithTimeleft(DNCDefinesData.Buffs.FlourishingFinish, 8000))
        {
            return 2;
        }
        if (!Core.Me.HasAura(DNCDefinesData.Buffs.Devilment))
        {
            return -2;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.Tillana.GetSpell());
    }
}