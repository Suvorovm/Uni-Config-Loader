using Config.Descriptor;
using Cysharp.Threading.Tasks;

namespace Config.Provider
{
    public interface ILoadConfigProvider
    {
        UniTask Init(ConfigLoadDescriptor loadDescriptor);
        
        T LoadConfig<T>(string configPath);
    }
}