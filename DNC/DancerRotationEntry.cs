using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.GUI;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Blz.DNC.Data;
using Blz.DNC.GCD;
using Blz.DNC.Opener;
using Blz.DNC.Setting;
using ImGuiNET;
using System.Runtime.CompilerServices;

namespace Blz.DNC;

public class DancerRotationEntry : IRotationEntry
{
    public void Dispose()
    {
        // TODO release managed resources here
    }

    
    
    public string AuthorName { get; set; } = "Blz";

    private List<SlotResolverData> SlotResolvers = new()
    {
        new SlotResolverData(new TecStepFinishGCD(), SlotMode.Gcd),
        new SlotResolverData(new DNCTecStepGCD(),SlotMode.Gcd),
        new SlotResolverData(new DNCStepGCD(),SlotMode.Gcd),
        new SlotResolverData(new DNCLastDanceGCD(),SlotMode.Gcd),
        new SlotResolverData(new DNCFinishingMoveGCD(),SlotMode.Gcd),
        new SlotResolverData(new DNCSaberDanceGCD(),SlotMode.Gcd),
        new SlotResolverData(new DNCTillanaGCD(),SlotMode.Gcd),
        new SlotResolverData(new DNCStarfallDanceGCD(),SlotMode.Gcd),
        //new SlotResolverData(new DNCDanceOfTheDawnGCD(),SlotMode.Gcd),
        new SlotResolverData(new DNCStdStepGCD(),SlotMode.Gcd),
        new SlotResolverData(new DNCProcsGCD(),SlotMode.Gcd),
        new SlotResolverData(new DNCBaseGCD(),SlotMode.Gcd),
        new SlotResolverData(new DNCDevilmentAbility(),SlotMode.OffGcd),
        new SlotResolverData(new DNCUsePotionAbility(),SlotMode.OffGcd),
        new SlotResolverData(new DNCFlourishAbility(),SlotMode.OffGcd),
        new SlotResolverData(new DNCFanDance4Ability(),SlotMode.OffGcd),
        new SlotResolverData(new DNCFanDance3Ability(),SlotMode.OffGcd),
        new SlotResolverData(new DNCFanDanceAbility(),SlotMode.OffGcd)
    };
    
    public Rotation Build(string settingFolder)
    {
        DNCSettings.Build(settingFolder);
        BuildQT();
        var rot = new Rotation(SlotResolvers)
        {
            TargetJob = Jobs.Dancer,
            AcrType = AcrType.HighEnd,
            MinLevel = 90,
            MaxLevel = 100,
            Description = "舞者测试",
        };
        //rot.AddSlotSequences(new DNCTecStepFinishSequence());
        rot.SetRotationEventHandler(new DancerRotationEventHandler());
        rot.AddOpener(new Func<uint, IOpener>(GetOpener));
        return rot;
    }

    private IOpener? GetOpener(uint level)
    {
        if (level < 70)
        {
            return null;
        }
        else
        {
            return new DNCStdOpener100();
        }
    }

    // 声明当前要使用的UI的实例 示例里使用QT
    public static JobViewWindow QT { get; private set; }
    
    // 如果你不想用QT 可以自行创建一个实现IRotationUI接口的类
    public IRotationUI GetRotationUI()
    {
        return DancerRotationEntry.QT;
    }
    
    // 构造函数里初始化QT
    public void BuildQT()
    {
        // JobViewSave是AE底层提供的QT设置存档类 在你自己的设置里定义即可
        // 第二个参数是你设置文件的Save类 第三个参数是QT窗口标题
        DancerRotationEntry.QT = new JobViewWindow(DNCSettings.Instance.JobViewSave, DNCSettings.Instance.Save, "Blz DNC");
        DancerRotationEntry.QT.AddTab("战斗", DrawBattle);
        DancerRotationEntry.QT.AddQt("爆发药", false, "吃药");
        DancerRotationEntry.QT.AddQt("AOE", true, "打AOE");
        DancerRotationEntry.QT.AddQt("爆发", true, "正常用资源");
        DancerRotationEntry.QT.AddQt("小舞", true, "标准舞步");
        DancerRotationEntry.QT.AddQt("大舞", true, "技巧舞步");
        DancerRotationEntry.QT.AddQt("百花", true, "百花争艳");
        DancerRotationEntry.QT.AddQt("燃尽爆发", false, "燃尽爆发");
        DancerRotationEntry.QT.AddQt("自动舞伴", true, "自动舞伴");
    }

    public void DrawBattle(JobViewWindow jobViewWindow)
    {

        ImGui.Checkbox("战斗结束qt自动重置回战斗前状态", ref DNCSettings.Instance.AutoReset);
        ImGuiHelper.LeftInputInt("剑舞触发阈值", ref DNCSettings.Instance.espritThreshold, 10, 100, 10);
        ImGui.Checkbox("2分钟爆发药", ref DNCSettings.Instance.delayPotion);
        //List<string> rotations = (List<string>)DNCSettings.DefaultConfig["rotations"];
        //ImGuiHelper.LeftCombo("选择起手:", ref DNCSettings.Instance.rotationIndex, rotations.ToArray(), 200);
        if (TTKHelper.IsTargetTTK(Core.Me.GetCurrTarget(), false)) { }
        if (ImGui.Button("保存设置"))
        {
            DNCSettings.Instance.Save();
        }

        /*        ImGui.Text(DNCDefinesData.Spells.TechnicalStep + "");
                ImGui.TextUnformatted($"大舞cd{DNCDefinesData.Spells.TechnicalStep.GetSpell().Cooldown}");
                ImGui.Text(DNCSpellHelper.OutSpell+"");
                ImGui.Text(Core.Me.GetCurrTarget().CanAttack() + "");
                ImGui.Text(AI.Instance.BattleData.CurrBattleTimeInMs + "");
                ImGui.Text(Core.Me.HasAura(DNCDefinesData.Buffs.TechnicalStep) + "");
                ImGui.Text(Core.Resolve<JobApi_Dancer>().CompleteSteps + "");
                if (ImGui.CollapsingHeader("插入技能状态"))
                {
                    if (ImGui.Button("清除队列"))
                    {
                        AI.Instance.BattleData.HighPrioritySlots_OffGCD.Clear();
                        AI.Instance.BattleData.HighPrioritySlots_GCD.Clear();
                    }
                    ImGui.Text("-------能力技-------");
                    if (AI.Instance.BattleData.HighPrioritySlots_OffGCD.Count > 0)
                    {
                        foreach (Spell spell in AI.Instance.BattleData.HighPrioritySlots_OffGCD)
                        {
                            ImGui.Text(spell.Name);
                        }
                    }
                    ImGui.Text("-------GCD-------");
                    if (AI.Instance.BattleData.HighPrioritySlots_GCD.Count > 0)
                    {
                        foreach (Spell spell2 in AI.Instance.BattleData.HighPrioritySlots_GCD)
                        {
                            ImGui.Text(spell2.Name);
                        }
                    }
                }
                if (ImGui.TreeNode("循环"))
                {
                    DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
                    defaultInterpolatedStringHandler.AppendLiteral("爆发药：");
                    defaultInterpolatedStringHandler.AppendFormatted<bool>(DancerRotationEntry.QT.GetQt("爆发药"));
                    ImGui.Text(defaultInterpolatedStringHandler.ToStringAndClear());
                    defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
                    defaultInterpolatedStringHandler.AppendLiteral("gcd时间：");
                    defaultInterpolatedStringHandler.AppendFormatted<int>(Core.Resolve<MemApiSpell>().GetElapsedGCD());
                    ImGui.Text(defaultInterpolatedStringHandler.ToStringAndClear());
                    defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
                    defaultInterpolatedStringHandler.AppendLiteral("gcd总时间：");
                    defaultInterpolatedStringHandler.AppendFormatted<int>(Core.Resolve<MemApiSpell>().GetGCDDuration());
                    ImGui.Text(defaultInterpolatedStringHandler.ToStringAndClear());
                    defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
                    defaultInterpolatedStringHandler.AppendLiteral("连击剩余时间：");
                    defaultInterpolatedStringHandler.AppendFormatted<double>(Core.Resolve<MemApiSpell>().GetComboTimeLeft().TotalMilliseconds);
                    ImGui.Text(defaultInterpolatedStringHandler.ToStringAndClear());
                    defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 1);
                    defaultInterpolatedStringHandler.AppendLiteral("战斗时间：");
                    defaultInterpolatedStringHandler.AppendFormatted<long>(AI.Instance.BattleData.CurrBattleTimeInMs);
                    ImGui.Text(defaultInterpolatedStringHandler.ToStringAndClear());
                    defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
                    defaultInterpolatedStringHandler.AppendLiteral("时间：");
                    defaultInterpolatedStringHandler.AppendFormatted<int>(SettingMgr.GetSetting<GeneralSettings>().ActionQueueInMs);
                    ImGui.Text(defaultInterpolatedStringHandler.ToStringAndClear());
                    ImGui.TreePop();
                }
                if (ImGui.TreeNode("自身信息"))
                {
                    DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
                    defaultInterpolatedStringHandler.AppendLiteral("是否在跳舞：");
                    defaultInterpolatedStringHandler.AppendFormatted<bool>(Core.Resolve<JobApi_Dancer>().IsDancing);
                    ImGui.Text(defaultInterpolatedStringHandler.ToStringAndClear());
                    defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
                    defaultInterpolatedStringHandler.AppendLiteral("完成的舞步：");
                    defaultInterpolatedStringHandler.AppendFormatted<int>(Core.Resolve<JobApi_Dancer>().CompleteSteps);
                    ImGui.Text(defaultInterpolatedStringHandler.ToStringAndClear());
                    defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
                    defaultInterpolatedStringHandler.AppendLiteral("当前幻扇值：");
                    defaultInterpolatedStringHandler.AppendFormatted<int>(Core.Resolve<JobApi_Dancer>().FourFoldFeathers);
                    ImGui.Text(defaultInterpolatedStringHandler.ToStringAndClear());
                    ImGui.TreePop();
                }
                if (ImGui.TreeNode("技能释放"))
                {
                    DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 1);
                    defaultInterpolatedStringHandler.AppendLiteral("上个技能：");
                    defaultInterpolatedStringHandler.AppendFormatted<uint>(Core.Resolve<MemApiSpellCastSuccess>().LastSpell);
                    ImGui.Text(defaultInterpolatedStringHandler.ToStringAndClear());
                    defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
                    defaultInterpolatedStringHandler.AppendLiteral("上个GCD：");
                    defaultInterpolatedStringHandler.AppendFormatted<uint>(Core.Resolve<MemApiSpellCastSuccess>().LastGcd);
                    ImGui.Text(defaultInterpolatedStringHandler.ToStringAndClear());
                    defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
                    defaultInterpolatedStringHandler.AppendLiteral("上个能力技：");
                    defaultInterpolatedStringHandler.AppendFormatted<uint>(Core.Resolve<MemApiSpellCastSuccess>().LastAbility);
                    ImGui.Text(defaultInterpolatedStringHandler.ToStringAndClear());
                    ImGui.TreePop();
                }
                if (ImGui.TreeNode("小队"))
                {
                    DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 1);
                    defaultInterpolatedStringHandler.AppendLiteral("小队人数：");
                    defaultInterpolatedStringHandler.AppendFormatted<int>(PartyHelper.CastableParty.Count);
                    ImGui.Text(defaultInterpolatedStringHandler.ToStringAndClear());
                    defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
                    defaultInterpolatedStringHandler.AppendLiteral("小队坦克数量：");
                    defaultInterpolatedStringHandler.AppendFormatted<int>(PartyHelper.CastableTanks.Count);
                    ImGui.Text(defaultInterpolatedStringHandler.ToStringAndClear());
                    ImGui.TreePop();
                }*/
    }

    public void OnDrawSetting(){
        DNCSettingsUI.Instance.Draw();
    }
}