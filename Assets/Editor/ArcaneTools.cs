using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneBuildManager : EditorWindow
{
    private static List<SceneAsset> viewScenes = new List<SceneAsset>();
    private static List<SceneAsset> padScenes = new List<SceneAsset>();

    private const string ViewScenesKey = "ViewScenesPaths";
    private const string PadScenesKey = "PadScenesPaths";

    [MenuItem("ArcaneTools/Scene Build Manager")]
    public static void ShowWindow()
    {
        GetWindow<SceneBuildManager>("Scene Build Manager");
        LoadScenes();
    }

    private static void LoadScenes()
    {
        viewScenes = LoadScenesFromPrefs(ViewScenesKey);
        padScenes = LoadScenesFromPrefs(PadScenesKey);
    }

    private static List<SceneAsset> LoadScenesFromPrefs(string key)
    {
        var paths = EditorPrefs.GetString(key, "").Split(';');
        return paths.Where(path => !string.IsNullOrEmpty(path))
                    .Select(path => AssetDatabase.LoadAssetAtPath<SceneAsset>(path))
                    .ToList();
    }

    private static void SaveScenesToPrefs(string key, List<SceneAsset> scenes)
    {
        var paths = scenes.Where(scene => scene != null)
                          .Select(scene => AssetDatabase.GetAssetPath(scene))
                          .ToArray();
        EditorPrefs.SetString(key, string.Join(";", paths));
    }

    void OnGUI()
    {
        GUILayout.Label("View Scenes", EditorStyles.boldLabel);
        DrawScenes(ref viewScenes, ViewScenesKey);

        GUILayout.Space(20);

        GUILayout.Label("Pad Scenes", EditorStyles.boldLabel);
        DrawScenes(ref padScenes, PadScenesKey);
    }

    void DrawScenes(ref List<SceneAsset> scenes, string prefsKey)
    {
        bool scenesUpdated = false;

        for (int i = 0; i < scenes.Count; i++)
        {
            var newScene = (SceneAsset)EditorGUILayout.ObjectField(scenes[i], typeof(SceneAsset), false);
            if (newScene != scenes[i])
            {
                scenes[i] = newScene;
                scenesUpdated = true;
            }
        }

        if (GUILayout.Button("Add Scene"))
        {
            scenes.Add(null);
            scenesUpdated = true;
        }

        if (GUILayout.Button("Set Scenes"))
        {
            SetScenes(scenes);
        }

        if (scenesUpdated)
        {
            SaveScenesToPrefs(prefsKey, scenes);
        }
    }

    private void SetScenes(List<SceneAsset> scenes)
    {
        var scenePaths = scenes.Where(scene => scene != null)
                               .Select(scene => AssetDatabase.GetAssetPath(scene))
                               .ToArray();
        var editorBuildSettingsScenes = scenePaths.Select(path => new EditorBuildSettingsScene(path, true)).ToArray();

        EditorBuildSettings.scenes = editorBuildSettingsScenes;
    }
}
