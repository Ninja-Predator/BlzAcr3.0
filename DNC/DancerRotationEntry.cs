using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using Blz.DNC.GCD;
using Blz.DNC.Setting;

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
            new SlotResolverData(new DNCTecStepGCD(),SlotMode.Gcd),
            new SlotResolverData(new DNCStepGCD(),SlotMode.Gcd),
            new SlotResolverData(new DNCSaberDanceGCD(),SlotMode.Gcd),
            new SlotResolverData(new DNCLastDanceGCD(),SlotMode.Gcd),
            new SlotResolverData(new DNCStarfallDanceGCD(),SlotMode.Gcd),
            new SlotResolverData(new DNCTillanaGCD(),SlotMode.Gcd),
            new SlotResolverData(new DNCFinishingMoveGCD(),SlotMode.Gcd),
            new SlotResolverData(new DNCDanceOfTheDawnGCD(),SlotMode.Gcd),
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

        return rot;
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
        DancerRotationEntry.QT.AddQt("爆发药", false, "吃药");
        DancerRotationEntry.QT.AddQt("AOE", true, "打AOE");
        DancerRotationEntry.QT.AddQt("爆发", true, "正常用资源");
        DancerRotationEntry.QT.AddQt("小舞", true, "标准舞步");
        DancerRotationEntry.QT.AddQt("大舞", true, "技巧舞步");
        DancerRotationEntry.QT.AddQt("百花", true, "百花争艳");
        DancerRotationEntry.QT.AddQt("燃尽爆发", false, "燃尽爆发");
        DancerRotationEntry.QT.AddQt("优先流星舞", false, "平时不推荐开启");
        DancerRotationEntry.QT.AddQt("自动舞伴", false, "自动舞伴");
    }

    public void OnDrawSetting(){}
}