using System;
using System.Collections.Generic;
using Config.Provider;

namespace Config.Factory
{
    public class ConfigLoadFactory
    {
        private Dictionary<string, Type> _providers = new Dictionary<string, Type>()
        {
            {"fireBaseLoader", typeof(FireBaseLoadConfigProvider)}
        };

        public ILoadConfigProvider CreateProvider(string providerTypeString)
        {
            Type providerType = _providers[providerTypeString];
            ILoadConfigProvider provider = (ILoadConfigProvider) Activator.CreateInstance(providerType);
            return provider;
        }
    }
}