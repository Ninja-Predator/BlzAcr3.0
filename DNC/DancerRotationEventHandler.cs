using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.AILoop;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Blz.DNC.Data;
using Dalamud.Game.ClientState.Objects.Types;

namespace Blz.DNC
{
    public class DancerRotationEventHandler : IRotationEventHandler
    {
        private static Dictionary<Jobs, int> jobPriorities = new(){
            { Jobs.Viper, 1 },
            { Jobs.Monk, 2 },
            { Jobs.Pictomancer, 3 },
            { Jobs.BlackMage, 4 },
            { Jobs.Reaper, 5 },
            { Jobs.Samurai, 6},
            { Jobs.Ninja, 7},
            { Jobs.Summoner, 8},
            { Jobs.Dragoon, 9},
            { Jobs.Machinist, 10},
            { Jobs.Bard, 11},
            { Jobs.Dancer, 12},
            { Jobs.DarkKnight, 13},
            { Jobs.Gunbreaker, 14},
            { Jobs.Paladin, 15},
            { Jobs.Warrior, 16},
            { Jobs.Sage, 17},
            { Jobs.WhiteMage, 18},
            { Jobs.Scholar, 19},
            { Jobs.Astrologian, 20}
        };
        public void AfterSpell(Slot slot, Spell spell)
        {
            if (spell == DNCDefinesData.Spells.QuadrupleTechnicalFinish.GetSpell())
            {
                if (DNCDefinesData.Spells.Devilment.IsUnlock() && (DNCDefinesData.Spells.Devilment.IsReady() || Core.Resolve<MemApiSpell>().GetCooldown(DNCDefinesData.Spells.Devilment).TotalMilliseconds < 1500.0))
                    slot.Add(DNCDefinesData.Spells.Devilment.GetSpell());
            }
        }

        public void OnBattleUpdate(int currTimeInMs)
        {

        }

        public void OnEnterRotation()
        {

        }

        public void OnExitRotation()
        {

        }

        public Task OnNoTarget()
        {
            if (Core.Resolve<JobApi_Dancer>().IsDancing)
            {
                if (Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.StandardStep))
                {
                    if (Core.Resolve<JobApi_Dancer>().CompleteSteps < 2)
                    {
                        Slot slot = new();
                        slot.Add(DNCSpellHelper.GetStep());
                        slot.Run(AI.Instance.BattleData, false);
                    }
                    else if (DancerRotationEntry.QT.GetQt("小舞") && TargetHelper.GetNearbyEnemyCount(10) > 0 && AI.Instance.BattleData.CurrBattleTimeInMs > 0)
                    {
                        Slot slot = new();
                        slot.Add(DNCDefinesData.Spells.DoubleStandardFinish.GetSpell());
                        slot.Run(AI.Instance.BattleData, false);
                    }

                }
                else if (Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.TechnicalStep))
                {
                    if (Core.Resolve<JobApi_Dancer>().CompleteSteps < 4)
                    {
                        Slot slot = new();
                        slot.Add(DNCSpellHelper.GetStep());
                        slot.Run(AI.Instance.BattleData, false);
                    }
                    else if (DancerRotationEntry.QT.GetQt("大舞") && TargetHelper.GetNearbyEnemyCount(10) > 0 && AI.Instance.BattleData.CurrBattleTimeInMs > 0)
                    {
                        Slot slot = new();
                        slot.Add(DNCDefinesData.Spells.QuadrupleTechnicalFinish.GetSpell());
                        slot.Run(AI.Instance.BattleData, false);
                    }

                }
            }
            else if (DancerRotationEntry.QT.GetQt("自动舞伴") && !Core.Me.HasLocalPlayerAura(DNCDefinesData.Buffs.ClosedPosition) && PartyHelper.Party.Count > 1 && DNCDefinesData.Spells.ClosedPosition.IsUnlock() && DNCDefinesData.Spells.ClosedPosition.IsReady())
            {
                IBattleChara targetPlayer = PartyHelper.Party[^1];
                foreach (var player in PartyHelper.Party)
                {
                    if (player != Core.Me && jobPriorities.TryGetValue(player.CurrentJob(), out var playerPriority) && playerPriority <= jobPriorities[targetPlayer.CurrentJob()])
                    {
                        if (targetPlayer.CurrentJob() == player.CurrentJob() && targetPlayer.MaxHp >= player.MaxHp)
                        {
                            break;
                        }
                        else
                        {
                            targetPlayer = player;
                        }
                    }
                }
                if (!targetPlayer.IsDead) {
                    SpellHelper.Cast(new Slot(), DNCDefinesData.Spells.ClosedPosition.GetSpell(), targetPlayer);
                }
            }
            return Task.CompletedTask;
        }

        public Task OnPreCombat()
        {
            return Task.CompletedTask;
        }

        public void OnResetBattle()
        {

        }

        public void OnSpellCastSuccess(Slot slot, Spell spell)
        {

        }

        public void OnTerritoryChanged()
        {

        }
    }
}
