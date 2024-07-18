using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using Blz.DNC.Data;

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
        if (!DNCDefinesData.Spells.FinishingMove.IsReady())
        {
            return -1;
        }
        if (Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.TechnicalFinish) && Core.Resolve<JobApi_Dancer>().Esprit > 75)
        {
            return -2;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(DNCDefinesData.Spells.FinishingMove.GetSpell());
    }
}