using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.Extension;
using AEAssist.Helper;
using Blz.DNC.Data;
using Blz.DNC.Setting;

namespace Blz.DNC.Opener
{
    public class DNCStdOpener100 : IOpener
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

        public List<Action<Slot>> Sequence { get; } = new List<Action<Slot>>()
        {
            Step0
        };

        public void InitCountDown(CountDownHandler countDownHandler)
        {
            countDownHandler.AddAction(14000,DNCDefinesData.Spells.StandardStep);
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
    }
}
