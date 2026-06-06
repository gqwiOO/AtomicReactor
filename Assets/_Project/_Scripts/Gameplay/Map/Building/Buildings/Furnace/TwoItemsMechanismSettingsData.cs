using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Map.Building.Items.Data;
using UnityEngine;

namespace Gameplay.Map.Building.Furnace
{
    [Serializable]
    public class TwoItemsMechanismSettingsData: BuildingSettingsData
    {
        public float EnergySpentForOneItem;
        public float BuildingEnergyPower;
        public float Speed;

        [field:SerializeField]
        public List<ItemTransformationData> ItemTransformationDataList { get; set; }

        public int GetOutputItemOfMechanism(int inputItemKey)
        {
            return ItemTransformationDataList.FirstOrDefault(i => i.InputItem.ItemId == inputItemKey)!
            .OutputItem.ItemId;
            // return ItemTransformationDataList.Count;
        }
    }

    [Serializable]
    public class ItemTransformationData
    {
        public ItemDataAsset InputItem;
        public ItemDataAsset OutputItem;
    }

    public abstract class BuildingSettingsData
    {
        
    }
}