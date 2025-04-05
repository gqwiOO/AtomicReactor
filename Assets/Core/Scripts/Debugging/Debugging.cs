using UnityEngine;

namespace Core.Scripts.Debugging
{
    public static class Debugging
    {
        private static DebuggingSettings _settings;

        public static void Init(DebuggingSettings settings)
        {
            _settings = settings;
        }

        public static void Log<T>(T obj, string message)
        {
            if(CanSend())
            {
                string sendMessage = $"[{nameof(T)}] {message}.";
                Debug.Log(sendMessage);
            }
        }
        public static void Error<T>(T obj, string message)
        {
            if(CanSend())
            {
                string sendMessage = $"[{nameof(T)}] {message}.";
                Debug.LogError(sendMessage);
            }
        }

        private static bool CanSend() 
            => _settings.IsEnabled;
    }
}