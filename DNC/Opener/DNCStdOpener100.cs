using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.Extension;
using AEAssist.Helper;
using Blz.DNC.Data;
using Blz.DNC.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blz.DNC.Opener
{
    public class DNCStdOpener100 : IOpener, ISlotSequence
    {
        public int StartCheck()
        {
            if (PartyHelper.Party.Count <= 4 && !Core.Me.GetCurrTarget().IsDummy() && !Core.Me.GetCurrTarget().IsBoss())
            {
                return -1;
            }
            if (!DNCDefinesData.Spells.TechnicalStep.IsReady())
            {
                return -4;
            }
            if (!DNCDefinesData.Spells.Devilment.IsReady())
            {
                return -5;
            }
            if (!DNCDefinesData.Spells.Flourish.IsReady())
            {
                return -6;
            }
            return 0;
        }

        public int StopCheck()
        {
            return -1;
        }

        public List<Action<Slot>> Sequence { get; }

        public void InitCountDown(CountDownHandler countDownHandler)
        {
            countDownHandler.AddAction(14000,DNCDefinesData.Spells.StandardStep,SpellTargetType.Self);
            countDownHandler.AddAction(11500, DNCSpellHelper.GetStep);
            countDownHandler.AddAction(9000, DNCSpellHelper.GetStep);
            if (DancerRotationEntry.QT.GetQt("爆发药") && !DNCSettings.Instance.delayPotion)
            {
                countDownHandler.AddPotionAction(2000);
            }
            countDownHandler.AddAction(500, DNCDefinesData.Spells.DoubleStandardFinish);
        }


        private static void Step0(Slot slot)
        {
            slot.Add(DNCDefinesData.Spells.Flourish.GetSpell());
            slot.Add(DNCDefinesData.Spells.TechnicalStep.GetSpell());
        }

        public DNCStdOpener100()
        {
            Sequence.Add(new Action<Slot>(Step0));

        }

    }
}
