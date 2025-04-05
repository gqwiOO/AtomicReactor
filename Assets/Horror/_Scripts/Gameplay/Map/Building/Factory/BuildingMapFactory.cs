using UnityEngine;
using Zenject;

namespace Gameplay.Map.Building.Factory
{
    public class BuildingMapFactory: MonoBehaviour, IBuildingMapFactory
    {
        [SerializeField] 
        private BuildingSettingsDataAssetsCollection _buildingsAssetCollection;
        
        private DiContainer _diContainer;

        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        public T CreateBuilding<T>(string key) where T : BuildingMapObject
        {
            T result = _diContainer.InstantiatePrefabForComponent<T>(_buildingsAssetCollection.GetByKey(key).BuildingMapObject);
            result.SetMaterial(Material.Instantiate(result.BaseMaterial));
            return result;
        }
    }

    public interface IBuildingMapFactory
    {
        T CreateBuilding<T>(string key) where T : BuildingMapObject;
    }
}