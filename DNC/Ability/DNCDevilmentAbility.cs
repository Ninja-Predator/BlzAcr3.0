using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Blz.DNC;
using Blz.DNC.Data;

namespace Blz.DNC.GCD;

public class DNCDevilmentAbility : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.Devilment.IsUnlock())
        {
            return -10;
        }
        if (!DNCDefinesData.Spells.Devilment.IsReady())
        {
            return -10;
        }
        if (!Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.TechnicalFinish))
        {
            return -1;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.Devilment.GetSpell());
    }
}