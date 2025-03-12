using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts.Extension.System
{
    public static class ListExtension
    {
        public static T PickRandom<T>(this IList<T> list) => list[Random.Range(0, list.Count)];
    }
}