using Gameplay.Buildings.Screens;
using Gameplay.Map.Building.Destroy;
using Gameplay.Map.Building.Selector;
using Gameplay.Map.Installer;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class BuildingDestroySetup
{
    [MenuItem("AtomicReactor/Setup Building Destroy Feature")]
    public static void Setup()
    {
        var scene = EditorSceneManager.GetActiveScene();
        if (!scene.name.Contains("Gameplay"))
        {
            EditorUtility.DisplayDialog("Wrong scene", "Please open the Gameplay scene first.", "OK");
            return;
        }

        var selector = SetupBuildingSelector();
        var handler = SetupBuildingDestroyHandler(selector);

        if (selector != null && handler != null)
            WireMapInstaller(selector, handler);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();

        Debug.Log("[BuildingDestroySetup] Done. Building destroy feature is wired up.");
    }

    static BuildingSelector SetupBuildingSelector()
    {
        var existing = Object.FindObjectOfType<BuildingSelector>();
        if (existing != null)
        {
            Debug.Log("[BuildingDestroySetup] BuildingSelector already exists, skipping.");
            return existing;
        }

        var cam = Camera.main;
        if (cam == null) { Debug.LogError("[BuildingDestroySetup] Main Camera not found."); return null; }

        var selector = cam.gameObject.AddComponent<BuildingSelector>();

        var so = new SerializedObject(selector);
        so.FindProperty("mainCamera").objectReferenceValue = cam;
        so.ApplyModifiedProperties();

        Debug.Log("[BuildingDestroySetup] BuildingSelector added to Main Camera.");
        return selector;
    }

    static BuildingDestroyHandler SetupBuildingDestroyHandler(BuildingSelector selector)
    {
        var existing = Object.FindObjectOfType<BuildingDestroyHandler>();
        if (existing != null)
        {
            Debug.Log("[BuildingDestroySetup] BuildingDestroyHandler already exists, skipping.");
            return existing;
        }

        var go = new GameObject("BuildingDestroyHandler");
        var handler = go.AddComponent<BuildingDestroyHandler>();

        var popup = CreateConfirmPopup();
        if (popup != null)
        {
            var so = new SerializedObject(handler);
            so.FindProperty("popup").objectReferenceValue = popup;
            so.ApplyModifiedProperties();
        }

        Debug.Log("[BuildingDestroySetup] BuildingDestroyHandler created.");
        return handler;
    }

    static ConfirmDestroyBuildingPopup CreateConfirmPopup()
    {
        var existing = Object.FindObjectOfType<ConfirmDestroyBuildingPopup>();
        if (existing != null) return existing;

        var canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null) { Debug.LogError("[BuildingDestroySetup] Canvas not found."); return null; }

        // Overlay panel (blocks input, darkens background)
        var overlayGO = new GameObject("ConfirmDestroyBuildingPopup");
        overlayGO.transform.SetParent(canvas.transform, false);
        var overlayRect = overlayGO.AddComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.offsetMin = Vector2.zero;
        overlayRect.offsetMax = Vector2.zero;
        var overlayImage = overlayGO.AddComponent<Image>();
        overlayImage.color = new Color(0f, 0f, 0f, 0.6f);
        overlayGO.AddComponent<GraphicRaycaster>();

        var popup = overlayGO.AddComponent<ConfirmDestroyBuildingPopup>();

        // Center panel
        var panelGO = new GameObject("Panel");
        panelGO.transform.SetParent(overlayGO.transform, false);
        var panelRect = panelGO.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(420, 180);
        var panelImage = panelGO.AddComponent<Image>();
        panelImage.color = new Color(0.15f, 0.15f, 0.15f, 0.95f);

        // Question text
        var textGO = new GameObject("QuestionText");
        textGO.transform.SetParent(panelGO.transform, false);
        var textRect = textGO.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0f, 0.45f);
        textRect.anchorMax = new Vector2(1f, 1f);
        textRect.offsetMin = new Vector2(15f, 0f);
        textRect.offsetMax = new Vector2(-15f, -10f);
        var tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text = "Do you really want to destroy\nthis building?";
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 20;

        // Confirm button (left)
        var confirmBtn = CreateButton(panelGO.transform, "ConfirmButton", "Destroy",
            new Vector2(-105f, -65f), new Vector2(175f, 45f), new Color(0.7f, 0.15f, 0.15f));

        // Cancel button (right)
        var cancelBtn = CreateButton(panelGO.transform, "CancelButton", "Cancel",
            new Vector2(105f, -65f), new Vector2(175f, 45f), new Color(0.2f, 0.2f, 0.2f));

        // Wire buttons
        var popupSO = new SerializedObject(popup);
        popupSO.FindProperty("confirmButton").objectReferenceValue = confirmBtn;
        popupSO.FindProperty("cancelButton").objectReferenceValue = cancelBtn;
        popupSO.ApplyModifiedProperties();

        // Start disabled
        overlayGO.SetActive(false);

        Debug.Log("[BuildingDestroySetup] ConfirmDestroyBuildingPopup created under Canvas.");
        return popup;
    }

    static Button CreateButton(Transform parent, string name, string label, Vector2 anchoredPos, Vector2 size, Color color)
    {
        var btnGO = new GameObject(name);
        btnGO.transform.SetParent(parent, false);
        var rect = btnGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = anchoredPos;
        rect.sizeDelta = size;
        var img = btnGO.AddComponent<Image>();
        img.color = color;
        var btn = btnGO.AddComponent<Button>();

        var textGO = new GameObject("Text");
        textGO.transform.SetParent(btnGO.transform, false);
        var textRect = textGO.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        var tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 18;
        tmp.color = Color.white;

        return btn;
    }

    static void WireMapInstaller(BuildingSelector selector, BuildingDestroyHandler handler)
    {
        var mapInstaller = Object.FindObjectOfType<MapInstaller>();
        if (mapInstaller == null) { Debug.LogError("[BuildingDestroySetup] MapInstaller not found."); return; }

        var so = new SerializedObject(mapInstaller);
        so.FindProperty("buildingSelector").objectReferenceValue = selector;
        so.FindProperty("buildingDestroyHandler").objectReferenceValue = handler;
        so.ApplyModifiedProperties();

        Debug.Log("[BuildingDestroySetup] MapInstaller wired.");
    }
}
