using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private GameObject loadingScreen;
    private float minimumLoadingTime = 1f;

    void Awake()
    {
        loadingScreen = GameObject.Find("loadingScreen");
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelAsync(levelName));
    }

    public void UnloadLevel(string levelName)
    {
        StartCoroutine(UnloadLevelAsync(levelName));
    }

    private IEnumerator LoadLevelAsync(string levelName)
    {
        ShowLoadingView(true);

        float startTime = Time.time;

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        while (!loadOperation.isDone)
        {
            // Optionally, update loading progress here
            yield return null;
        }

        while (Time.time - startTime < minimumLoadingTime)
        {
            yield return null;
        }

        ShowLoadingView(false);
    }

    private IEnumerator UnloadLevelAsync(string levelName)
    {
        ShowLoadingView(true);

        float startTime = Time.time;

        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(levelName);
        while (!unloadOperation.isDone)
        {
            // Optionally, update loading progress here
            yield return null;
        }

        while (Time.time - startTime < minimumLoadingTime)
        {
            yield return null;
        }

        ShowLoadingView(false);
    }

    private void ShowLoadingView(bool show)
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(show);
        }
    }
}
