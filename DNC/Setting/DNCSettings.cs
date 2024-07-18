using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Helper;
using AEAssist.IO;

namespace Blz.DNC.Setting;

/// <summary>
/// 配置文件适合放一些一般不会在战斗中随时调整的开关数据
/// 如果一些开关需要在战斗中调整 或者提供给时间轴操作 那就用QT
/// 非开关类型的配置都放配置里 比如诗人绝峰能量配置
/// </summary>
public class DNCSettings
{
    public static DNCSettings Instance;

    #region 标准模板代码 可以直接复制后改掉类名即可
    private static string path;
    public static void Build(string settingPath)
    {
        path = Path.Combine(settingPath,nameof(DNCSettings), ".json");
        if (!File.Exists(path))
        {
            Instance = new DNCSettings();
            Instance.Save();
            return;
        }
        try
        {
            Instance = JsonHelper.FromJson<DNCSettings>(File.ReadAllText(path));
        }
        catch (Exception e)
        {
            Instance = new();
            LogHelper.Error(e.ToString());
        }
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, JsonHelper.ToJson(this));
    }
    #endregion
    
    public JobViewSave JobViewSave = new(); // QT设置存档

    public int espritThreshold = 85;

    public bool delayPotion;

    // Token: 0x040003B1 RID: 945
    public Dictionary<string, object> JobWindowSetting = new Dictionary<string, object>();

    // Token: 0x040003B2 RID: 946
    public bool AutoReset = true;

}