using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private GameObject loadingScreen;

    void Awake()
    {
        loadingScreen = GameObject.Find("loadingScreen");
    }

    public IEnumerator LoadLevel(string levelName)
    {
        loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        loadingScreen.SetActive(false);
    }

    public IEnumerator UnloadLevel(string levelName)
    {
        if (SceneManager.GetSceneByName(levelName).isLoaded)
        {
            loadingScreen.SetActive(true);

            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(levelName);

            while (!asyncUnload.isDone)
            {
                yield return null;
            }

            loadingScreen.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"Attempted to unload {levelName}, but it is not currently loaded.");
        }
    }


}
