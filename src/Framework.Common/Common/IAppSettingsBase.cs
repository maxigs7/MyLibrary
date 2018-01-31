namespace Framework.Common
{
    public interface IAppSettingsBase
    {
        bool EnableOptimizations { get; }
        string PathBase { get; }
    }
}