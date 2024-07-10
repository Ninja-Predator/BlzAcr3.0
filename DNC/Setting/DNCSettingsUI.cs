using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.GUI;
using AEAssist.Helper;
using AEAssist.IO;
using AEAssist.MemoryApi;
using ImGuiNET;
using static Dalamud.Interface.Utility.Raii.ImRaii;
using System.Runtime.CompilerServices;
using System.Numerics;
using AEAssist.JobApi;

namespace Blz.DNC.Setting;

/// <summary>
/// 配置文件适合放一些一般不会在战斗中随时调整的开关数据
/// 如果一些开关需要在战斗中调整 或者提供给时间轴操作 那就用QT
/// 非开关类型的配置都放配置里 比如诗人绝峰能量配置
/// </summary>
public class DNCSettingsUI
{
    public static DNCSettingsUI Instance = new();
    public DNCSettings DNCSettings => DNCSettings.Instance;

    public void Draw()
    {
        ImGui.Dummy(new Vector2(0f, 5f));
        if (ImGui.BeginTabBar("###tab"))
        {
            if (ImGui.BeginTabItem("通常"))
            {
                ImGui.BeginChild("##tab1", new Vector2(0f, 0f));
                if (ImGui.CollapsingHeader("战斗设置"))
                {
                    ImGui.Checkbox("战斗结束qt自动重置回战斗前状态", ref DNCSettings.Instance.AutoReset);
                    ImGuiHelper.LeftInputInt("剑舞触发阈值", ref DNCSettings.Instance.espritThreshold, 10, 100, 10);
                    ImGuiHelper.ToggleButton("2分钟爆发药", ref DNCSettings.Instance.delayPotion);
                    //List<string> rotations = (List<string>)DNCSettings.DefaultConfig["rotations"];
                    //ImGuiHelper.LeftCombo("选择起手:", ref DNCSettings.Instance.rotationIndex, rotations.ToArray(), 200);
                    if (TTKHelper.IsTargetTTK(Core.Me.GetCurrTarget(), false)){}
                    if (ImGui.Button("保存设置"))
                    {
                        DNCSettings.Instance.Save();
                    }
                }
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
                ImGui.EndChild();
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("时间轴信息"))
            {
                ImGui.BeginChild("##tab2", new Vector2(0f, 0f));
                TriggerLine currTriggerline = AI.Instance.TriggerlineData.CurrTriggerLine;
                string notice = "NULL";
                if (currTriggerline != null)
                {
                    notice = currTriggerline.Author + " - " + currTriggerline.Name;
                }
                ImGui.Text("Triggerline: " + notice);
                if (currTriggerline != null)
                {
                    ImGui.Text("导出变量:");
                    foreach (string v in currTriggerline.ExposedVars)
                    {
                        int oldValue = AI.Instance.TriggerlineData.Variable.GetValueOrDefault(v);
                        ImGui.Text(v ?? "");
                        ImGui.InputInt("", ref oldValue);
                        AI.Instance.TriggerlineData.Variable[v] = oldValue;
                    }
                }
                ImGui.EndChild();
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("Dev"))
            {
                ImGui.BeginChild("##tab3", new Vector2(0f, 0f));
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
                    defaultInterpolatedStringHandler.AppendLiteral("下一个舞步：");
                    defaultInterpolatedStringHandler.AppendFormatted<uint>(Core.Resolve<JobApi_Dancer>().NextStep);
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
                }
                ImGui.EndChild();
                ImGui.EndTabItem();
            }
            ImGui.EndTabBar();
        }
        ImGui.End();
        ImGui.PopStyleColor();
        ImGui.PopStyleVar();
    }

}