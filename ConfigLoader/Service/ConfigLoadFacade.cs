using CGK.Descriptor.Service;
using Config.Descriptor;
using Config.Factory;
using Config.Provider;
using Cysharp.Threading.Tasks;

namespace Config.Service
{
    public class ConfigLoadFacade
    {
        private readonly ConfigLoadDescriptor _configLoadDescriptor;
        private readonly ILoadConfigProvider _configProvider;
        
        public ConfigLoadFacade(DescriptorHolder descriptorHolder)
        {
            ConfigLoadFactory configLoadFactory = new ConfigLoadFactory();
            _configLoadDescriptor = descriptorHolder.GetDescriptor<ConfigLoadDescriptor>();
            _configProvider = configLoadFactory.CreateProvider(_configLoadDescriptor.ActiveProvider);
        }

        public async UniTask Init()
        {
            await _configProvider.Init(_configLoadDescriptor);
        }

        public T LoadConfig<T>(string path)
        {
            return _configProvider.LoadConfig<T>(path);
        }
    }
}