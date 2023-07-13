using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using CGK.Utils;
using Config.Descriptor;
using Config.Error;
using Cysharp.Threading.Tasks;
using Firebase;
using UnityEngine;

namespace Config.Provider
{
    /// <summary>
    /// Get config from Remote config Firebase.  The Config must be XML. The Config-Key  in Firebase
    /// console is  <code>ConfigName + Application Version</code>
    /// </summary>
    public class FireBaseLoadConfigProvider : ILoadConfigProvider
    {
        private bool _initedCorrected;

        public async UniTask Init(ConfigLoadDescriptor loadDescriptor)
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();

            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                // Crashlytics will use the DefaultInstance, as well;
                // this ensures that Crashlytics is initialized.
                FirebaseApp app = FirebaseApp.DefaultInstance;
                Debug.Log(System.String.Format(
                    "All ok", dependencyStatus));
                // Set a flag here to indicate that your project is ready to use Firebase.
            }
            else
            {
                Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                throw new ConfigLoadException("Firebase init error ");
            }

            await Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync().AsUniTask();
            await Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync().AsUniTask();
            _initedCorrected = true;
        }

        public T LoadConfig<T>(string configPath)
        {
            if (!_initedCorrected)
            {
                throw new ConfigLoadException("Firebase Provider not Init");
            }

            string stringValue = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(configPath)
                .StringValue;
            if (stringValue.IsNullOrEmpty())
            {
                return default;
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(stringValue);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T loadedConfig;
            using (TextReader textReader = new StringReader(doc.OuterXml))
            {
                loadedConfig = (T)xmlSerializer.Deserialize(textReader);
            }
            return loadedConfig;
        }
    }
}