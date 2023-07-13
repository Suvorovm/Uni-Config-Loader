namespace Config.Error
{
    public class ConfigLoadException : System.Exception
    {
        public ConfigLoadException(string message) : base(message)
        {
        }
    }
}