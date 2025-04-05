using System;

namespace Core.Scripts.Extension.System
{
    public static class EnumExtension
    {
        public static T GetRandomExceptZero<T>() where T : Enum
        {
            Random random = new Random();
            var values = (T[])Enum.GetValues(typeof(T));
            var filteredValues = Array.FindAll(values, v => Convert.ToInt32(v) != 0);
            return filteredValues[random.Next(filteredValues.Length)];
        }
    }
}