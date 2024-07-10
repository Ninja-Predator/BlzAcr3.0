using AEAssist;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using AEAssist.Helper;
using AEAssist.IO;
using ImGuiNET;

namespace Blz.DNC.Trigger;

/// <summary>
/// 配置文件适合放一些一般不会在战斗中随时调整的开关数据
/// 如果一些开关需要在战斗中调整 或者提供给时间轴操作 那就用QT
/// 非开关类型的配置都放配置里 比如诗人绝峰能量配置
/// </summary>
public class DNCTrigger
{
    public string DisplayName { get; } = "DNC/QT";
    public string Remark { get; set; }

    public string Key = "";
    public bool Value;

    // 辅助数据 因为是private 所以不存档
    private int _selectIndex;
    private string[] _qtArray;

    public DNCTrigger()
    {
        _qtArray = DancerRotationEntry.QT.GetQtArray();
    }

    public bool Draw()
    {
        _selectIndex = Array.IndexOf(_qtArray, Key);
        if (_selectIndex == -1)
        {
            _selectIndex = 0;
        }
        ImGuiHelper.LeftCombo("选择Key", ref _selectIndex, _qtArray);
        Key = _qtArray[_selectIndex];
        ImGui.SameLine();
        using (new GroupWrapper())
        {
            ImGui.Checkbox("", ref Value);
        }
        return true;
    }

    public bool Handle()
    {
        DancerRotationEntry.QT.SetQt(Key, Value);
        return true;
    }

}