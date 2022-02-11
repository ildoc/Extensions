using System.Threading.Tasks;

namespace Infrastructure.Settings
{
    public interface IApplicationSettingsManager
    {
        bool Exists(string key);
        string GetSetting(string key, string ifDefault = default);
        Task<string> SaveSetting(string key, string value);
    }
}
