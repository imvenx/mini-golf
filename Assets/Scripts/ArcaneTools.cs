using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneBuildManager : EditorWindow
{
    private static List<SceneAsset> app1Scenes = new List<SceneAsset>();
    private static List<SceneAsset> app2Scenes = new List<SceneAsset>();

    [MenuItem("ArcaneTools/Scene Build Manager")]
    public static void ShowWindow()
    {
        GetWindow<SceneBuildManager>("Scene Build Manager");
    }

    void OnGUI()
    {
        GUILayout.Label("View Scenes", EditorStyles.boldLabel);
        for (int i = 0; i < app1Scenes.Count; i++)
        {
            app1Scenes[i] = (SceneAsset)EditorGUILayout.ObjectField(app1Scenes[i], typeof(SceneAsset), false);
        }

        if (GUILayout.Button("Add Scene for View"))
        {
            app1Scenes.Add(null);
        }

        if (GUILayout.Button("Set Scenes for View"))
        {
            SetScenes(app1Scenes);
        }

        GUILayout.Space(20);

        GUILayout.Label("Pad Scenes", EditorStyles.boldLabel);
        for (int i = 0; i < app2Scenes.Count; i++)
        {
            app2Scenes[i] = (SceneAsset)EditorGUILayout.ObjectField(app2Scenes[i], typeof(SceneAsset), false);
        }

        if (GUILayout.Button("Add Scene for Pad"))
        {
            app2Scenes.Add(null);
        }

        if (GUILayout.Button("Set Scenes for Pad"))
        {
            SetScenes(app2Scenes);
        }
    }

    private void SetScenes(List<SceneAsset> scenes)
    {
        var scenePaths = scenes.Where(scene => scene != null).Select(scene => AssetDatabase.GetAssetPath(scene)).ToArray();
        var editorBuildSettingsScenes = new EditorBuildSettingsScene[scenePaths.Length];

        for (int i = 0; i < scenePaths.Length; i++)
        {
            editorBuildSettingsScenes[i] = new EditorBuildSettingsScene(scenePaths[i], true);
        }

        EditorBuildSettings.scenes = editorBuildSettingsScenes;
    }
}
