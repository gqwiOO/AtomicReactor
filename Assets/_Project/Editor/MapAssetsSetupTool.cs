using GameAssembly.Horror._Scripts.Gameplay.Map.Biomes;
using Gameplay.Map.Cell;
using Gameplay.Map.Cell.Data;
using Gameplay.Map.Creator;
using Gameplay.Map.Generating.Ore;
using UnityEditor;
using UnityEngine;

public static class MapAssetsSetupTool
{
    private const string OresPath = "Assets/_Project/_ScriptableObjects/Map/Ores";
    private const string BiomesPath = "Assets/_Project/_ScriptableObjects/Map/Biomes";
    private const string CellPrefabsPath = "Assets/_Project/_Prefabs/Map/Cells";
    private const string DictAssetPath = "Assets/_Project/_ScriptableObjects/Map/CellsAssetCollectionDictionary.asset";

    [MenuItem("AtomicReactor/Setup Map Assets")]
    public static void CreateAllAssets()
    {
        EnsureFolder(OresPath);
        EnsureFolder(BiomesPath);

        var ironOre = CreateOrLoad<OreGenerationConfig>($"{OresPath}/IronOreConfig.asset");
        ironOre.OreType = CellType.IronOre;
        ironOre.MaxVeinSize = 25;
        ironOre.VeinDensity = 0.04f;
        EditorUtility.SetDirty(ironOre);

        var goldOre = CreateOrLoad<OreGenerationConfig>($"{OresPath}/GoldOreConfig.asset");
        goldOre.OreType = CellType.GoldOre;
        goldOre.MaxVeinSize = 15;
        goldOre.VeinDensity = 0.02f;
        EditorUtility.SetDirty(goldOre);

        var uraniumOre = CreateOrLoad<OreGenerationConfig>($"{OresPath}/UraniumOreConfig.asset");
        uraniumOre.OreType = CellType.UraniumOre;
        uraniumOre.MaxVeinSize = 10;
        uraniumOre.VeinDensity = 0.01f;
        EditorUtility.SetDirty(uraniumOre);

        var forestConfig = CreateOrLoad<BiomeConfig>($"{BiomesPath}/ForestBiomeConfig.asset");
        forestConfig.BiomeType = BiomeType.Forest;
        forestConfig.SpawnWeight = 3f;
        forestConfig.GroundCellType = CellType.Grass;
        forestConfig.CanSpawnTrees = true;
        forestConfig.TreeNoiseFrequency = 0.2f;
        forestConfig.TreeDensity = 0.4f;
        forestConfig.OreConfigs = System.Array.Empty<OreGenerationConfig>();
        EditorUtility.SetDirty(forestConfig);

        var fieldConfig = CreateOrLoad<BiomeConfig>($"{BiomesPath}/FieldBiomeConfig.asset");
        fieldConfig.BiomeType = BiomeType.Field;
        fieldConfig.SpawnWeight = 3f;
        fieldConfig.GroundCellType = CellType.Grass;
        fieldConfig.CanSpawnTrees = false;
        fieldConfig.OreConfigs = System.Array.Empty<OreGenerationConfig>();
        EditorUtility.SetDirty(fieldConfig);

        var oceanConfig = CreateOrLoad<BiomeConfig>($"{BiomesPath}/OceanBiomeConfig.asset");
        oceanConfig.BiomeType = BiomeType.Ocean;
        oceanConfig.SpawnWeight = 1f;
        oceanConfig.GroundCellType = CellType.Water;
        oceanConfig.CanSpawnTrees = false;
        oceanConfig.OreConfigs = System.Array.Empty<OreGenerationConfig>();
        EditorUtility.SetDirty(oceanConfig);

        var rockyConfig = CreateOrLoad<BiomeConfig>($"{BiomesPath}/RockyPlainBiomeConfig.asset");
        rockyConfig.BiomeType = BiomeType.RockyPlain;
        rockyConfig.SpawnWeight = 2f;
        rockyConfig.GroundCellType = CellType.Stone;
        rockyConfig.CanSpawnTrees = false;
        rockyConfig.OreConfigs = new OreGenerationConfig[] { ironOre, goldOre, uraniumOre };
        EditorUtility.SetDirty(rockyConfig);

        var collection = CreateOrLoad<BiomesCollection>($"{BiomesPath}/BiomesCollection.asset");
        collection.SiteDensityDivisor = 1500;
        collection.RelaxPasses = 5;
        collection.Biomes = new BiomeConfig[] { forestConfig, fieldConfig, oceanConfig, rockyConfig };
        EditorUtility.SetDirty(collection);

        CreatePrefabIfMissing($"{CellPrefabsPath}/IronOre.prefab", $"{CellPrefabsPath}/GoldOre.prefab");
        CreatePrefabIfMissing($"{CellPrefabsPath}/IronOre.prefab", $"{CellPrefabsPath}/UraniumOre.prefab");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        RegisterNewCellPrefabs();

        Debug.Log("[MapAssetsSetupTool] Done. Assign BiomesCollection to MapCreator, OreGenerator, WoodGenerator in the scene.");
    }

    private static void RegisterNewCellPrefabs()
    {
        var dict = AssetDatabase.LoadAssetAtPath<CellsAssetCollectionDictionary>(DictAssetPath);
        if (dict == null)
        {
            Debug.LogError("[MapAssetsSetupTool] CellsAssetCollectionDictionary not found at: " + DictAssetPath);
            return;
        }

        TryAddPrefabToDict(dict, $"{CellPrefabsPath}/GoldOre.prefab", CellType.GoldOre);
        TryAddPrefabToDict(dict, $"{CellPrefabsPath}/UraniumOre.prefab", CellType.UraniumOre);

        EditorUtility.SetDirty(dict);
        AssetDatabase.SaveAssets();
    }

    private static void TryAddPrefabToDict(CellsAssetCollectionDictionary dict, string prefabPath, CellType cellType)
    {
        if (dict.CellPrefabs.ContainsKey(cellType))
            return;

        var go = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (go == null)
        {
            Debug.LogWarning($"[MapAssetsSetupTool] Prefab not found: {prefabPath}");
            return;
        }

        var comp = go.GetComponent<CellComponent>();
        if (comp == null)
        {
            Debug.LogWarning($"[MapAssetsSetupTool] CellComponent missing on {prefabPath}");
            return;
        }

        dict.CellPrefabs[cellType] = comp;
    }

    private static T CreateOrLoad<T>(string path) where T : ScriptableObject
    {
        var existing = AssetDatabase.LoadAssetAtPath<T>(path);
        if (existing != null)
            return existing;
        var asset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(asset, path);
        return asset;
    }

    private static void CreatePrefabIfMissing(string sourcePath, string destPath)
    {
        if (AssetDatabase.LoadAssetAtPath<GameObject>(destPath) != null)
            return;
        AssetDatabase.CopyAsset(sourcePath, destPath);
    }

    private static void EnsureFolder(string path)
    {
        var parts = path.Split('/');
        var current = parts[0];
        for (int i = 1; i < parts.Length; i++)
        {
            var next = current + "/" + parts[i];
            if (!AssetDatabase.IsValidFolder(next))
                AssetDatabase.CreateFolder(current, parts[i]);
            current = next;
        }
    }
}
