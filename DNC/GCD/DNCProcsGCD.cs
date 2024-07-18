using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using Blz.DNC.Data;

namespace Blz.DNC.GCD;

public class DNCProcsGCD : ISlotResolver
{
    public int Check()
    {
        if (!DNCDefinesData.Spells.ReverseCascade.IsUnlock())
        {
            return -10;
        }
        if (!Core.Me.HasAura(DNCDefinesData.Buffs.FlourishingSymmetry) && !Core.Me.HasAura(DNCDefinesData.Buffs.FlourshingFlow) && !Core.Me.HasAura(DNCDefinesData.Buffs.SilkenFlow) && !Core.Me.HasAura(DNCDefinesData.Buffs.SilkenSymmetry))
        {
            return -1;
        }
        if (Core.Resolve<JobApi_Dancer>().FourFoldFeathers == 4)
        {
            return -2;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCSpellHelper.GetProcGCDCombo());
    }
}