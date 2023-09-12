namespace MixApp.Shared.Models;

public class Software
{
    /// <summary>
    /// 包定义
    /// </summary>
    public string? PackageIdentifier { get; set; }

    /// <summary>
    /// 包名称
    /// </summary>
    public string? PackageName { get; set; }
    
    /// <summary>
    /// 发布者
    /// </summary>
    public string? Publisher { get; set; }

    /// <summary>
    /// 短简介
    /// </summary>
    public string? ShortDescription { get; set; }

    /// <summary>
    /// 默认区域
    /// </summary>
    public string? DefaultLocale { get; set; }

    /// <summary>
    /// 官网
    /// </summary>
    public string? PackageUrl { get; set; }

    /// <summary>
    /// 封面
    /// </summary>
    public string? Cover { get; set; }
}