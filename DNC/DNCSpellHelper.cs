using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Blz.DNC;
using Blz.DNC.Data;

namespace Blz.DNC
{
    public static class DNCSpellHelper
    {
        public static uint OutSpell = 0U;

        public static Spell GetBaseGCDCombo()
        {
            if (DNCDefinesData.Spells.Windmill.IsUnlock() && TargetHelper.CheckNeedUseAoeByMe(5, 5, 3) && DancerRotationEntry.QT.GetQt("AOE"))
            {
                return DNCSpellHelper.GetAOECombo();
            }
            return DNCSpellHelper.GetSingleCombo();
        }

        private static Spell GetSingleCombo()
        {
            if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == DNCDefinesData.Spells.Cascade && DNCDefinesData.Spells.Fountain.IsUnlock())
            {
                return DNCDefinesData.Spells.Fountain.GetSpell();
            }
            return DNCDefinesData.Spells.Cascade.GetSpell();
        }

        private static Spell GetAOECombo()
        {
            if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == DNCDefinesData.Spells.Windmill && DNCDefinesData.Spells.Bladeshower.IsUnlock())
            {
                return DNCDefinesData.Spells.Bladeshower.GetSpell();
            }
            return DNCDefinesData.Spells.Windmill.GetSpell();
        }
        public static Spell GetProcGCDCombo()
        {
            if (DNCDefinesData.Spells.RisingWindmill.IsUnlock() && TargetHelper.CheckNeedUseAoeByMe(5, 5, 2) && DancerRotationEntry.QT.GetQt("AOE"))
            {
                return DNCSpellHelper.GetProcAOECombo();
            }
            return DNCSpellHelper.GetProcSingleCombo();
        }

        public static Spell GetProcAOECombo()
        {
            if (!Core.Me.HasAura(DNCDefinesData.Buffs.FlourshingFlow) && !Core.Me.HasAura(DNCDefinesData.Buffs.SilkenFlow))
            {
                return DNCDefinesData.Spells.RisingWindmill.GetSpell();
            }
            if (DNCDefinesData.Spells.Bloodshower.IsUnlock())
            {
                return DNCDefinesData.Spells.Bloodshower.GetSpell();
            }
            return null;
        }


        public static Spell GetProcSingleCombo()
        {
            if (!DancerRotationEntry.QT.GetQt("爆发"))
            {
                if (Core.Me.HasAura(DNCDefinesData.Buffs.SilkenFlow) && DNCDefinesData.Spells.Fountainfall.IsUnlock())
                {
                    return DNCDefinesData.Spells.Fountainfall.GetSpell();
                }
                if (Core.Me.HasAura(DNCDefinesData.Buffs.SilkenSymmetry) && DNCDefinesData.Spells.ReverseCascade.IsUnlock())
                {
                    return DNCDefinesData.Spells.ReverseCascade.GetSpell();
                }
            }
            if (!Core.Me.HasAura(DNCDefinesData.Buffs.FlourshingFlow) && !Core.Me.HasAura(DNCDefinesData.Buffs.SilkenFlow))
            {
                return DNCDefinesData.Spells.ReverseCascade.GetSpell();
            }
            if (DNCDefinesData.Spells.Fountainfall.IsUnlock())
            {
                return DNCDefinesData.Spells.Fountainfall.GetSpell();
            }
            return null;
        }

        public static Spell? GetStep()
        {
            if (Core.Me.GetCurrTarget().CanAttack())
            {
                if (Core.Me.HasAura(DNCDefinesData.Buffs.StandardStep) && Core.Resolve<JobApi_Dancer>().CompleteSteps == 2)
                {
                    return DNCDefinesData.Spells.DoubleStandardFinish.GetSpell();
                }
            }
            else
            {
                if (Core.Me.HasAura(DNCDefinesData.Buffs.StandardStep) && Core.Resolve<JobApi_Dancer>().CompleteSteps == 2)
                {
                    return null;
                }
                if (Core.Resolve<JobApi_Dancer>().CompleteSteps == 4)
                {
                    return null;
                }
            }
            return Core.Resolve<JobApi_Dancer>().NextStep.GetSpell();
        }
    }
}
