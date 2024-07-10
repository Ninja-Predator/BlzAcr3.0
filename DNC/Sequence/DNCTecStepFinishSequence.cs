using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Blz.DNC.Data;
using Blz.DNC.Setting;

namespace Blz.DNC.Opener
{
    public class DNCTecStepFinishSequence : ISlotSequence
    {
        public int StartCheck()
        {
            if (Core.Resolve<JobApi_Dancer>().CompleteSteps == 4 && Core.Resolve<JobApi_Dancer>().IsDancing)
            {
                return 1;
            }
            return -1;
        }

        public int StopCheck()
        {
            return -1;
        }

        public List<Action<Slot>> Sequence { get; } = new List<Action<Slot>>() { Step0 };




        private static void Step0(Slot slot)
        {
            slot.Add(DNCDefinesData.Spells.TechnicalStep.GetSpell());
            if(DNCDefinesData.Spells.Devilment.IsUnlock() && (DNCDefinesData.Spells.Devilment.IsReady()||Core.Resolve<MemApiSpell>().GetCooldown(DNCDefinesData.Spells.Devilment).TotalMilliseconds<1500.0))
                slot.Add(DNCDefinesData.Spells.Devilment.GetSpell());
        }

    }
}
