using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Map.Cell.Data
{
    [CreateAssetMenu(fileName = "CellsAssetCollectionDictionary", menuName = "Core/Map/Cell/CellsAssetCollectionDictionary")]
    public class CellsAssetCollectionDictionary: SerializedScriptableObject
    {
        [field: SerializeField] 
        public Dictionary<CellType,CellComponent> CellPrefabs { get; private set; }

        public CellComponent GetCellPrefab(CellType cellType)
        {
            return CellPrefabs[cellType];
        }
    }
}