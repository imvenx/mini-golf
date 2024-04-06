using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // private GameObject loadingScreen;
    private float minimumLoadingTimeSeconds = 1f;
    private string currentLevel;
    public delegate void LevelLoadedHandler();
    public event LevelLoadedHandler levelLoaded;

    public delegate void LevelUnloadedHandler();
    public event LevelUnloadedHandler levelUnloaded;

    void Awake()
    {
        // loadingScreen = GameObject.Find("Canvas").transform.Find("loadingScreen").gameObject;
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelAsync(levelName));
    }

    public void UnloadLevel()
    {
        StartCoroutine(UnloadLevelAsync());
    }

    private IEnumerator LoadLevelAsync(string levelName)
    {
        // ShowLoadingView(true);

        var startTime = Time.time;

        var loadOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        loadOperation.allowSceneActivation = false;

        yield return new WaitUntil(() => loadOperation.progress >= 0.9f && (Time.time - startTime) >= minimumLoadingTimeSeconds);

        loadOperation.allowSceneActivation = true;

        yield return new WaitUntil(() => loadOperation.isDone);

        currentLevel = levelName;

        levelLoaded.Invoke();

        // ShowLoadingView(false);
    }

    private IEnumerator UnloadLevelAsync()
    {
        if (string.IsNullOrEmpty(currentLevel)) yield return null;

        // ShowLoadingView(true);

        var startTime = Time.time;

        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentLevel);

        yield return new WaitUntil(() => unloadOperation.isDone && (Time.time - startTime) >= minimumLoadingTimeSeconds);

        currentLevel = "";

        levelUnloaded.Invoke();

        // ShowLoadingView(false);
    }


    // private void ShowLoadingView(bool show)
    // {
    //     loadingScreen.SetActive(show);
    // }
}
