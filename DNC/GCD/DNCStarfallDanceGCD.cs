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
        if (DancerRotationEntry.QT.GetQt("优先流星舞") || (Core.Me.HasAura(DNCDefinesData.Buffs.FlourishingFinish) && !Core.Me.HasAura(DNCDefinesData.Buffs.FlourishingFinish,16000)))
        {
            return 0;
        }
        if (!DancerRotationEntry.QT.GetQt("优先流星舞") && Core.Me.HasAura(DNCDefinesData.Buffs.FlourishingFinish,12000))
        {
            return -1;
        }
        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.StarfallDance.GetSpell());
    }
}