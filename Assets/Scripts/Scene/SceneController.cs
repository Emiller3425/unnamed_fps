using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static int activeSceneIndex;
    private void OnEnable()
    {
        GameEvents.current.OnPlayerDeath += ResetScene;
        GameEvents.current.OnLevelEnd += LoadNextScene;
    }

    private void Start()
    {
        activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    public void LoadSceneAsync(int index)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(index));
    }

    public IEnumerator LoadSceneAsyncCoroutine(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            yield return null;
        }
    }
    private void LoadNextScene()
    {
        activeSceneIndex += 1;
        LoadSceneAsync(activeSceneIndex);

    }
    private void ResetScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
    private void OnDisable()
    {
        GameEvents.current.OnPlayerDeath -= ResetScene;
        GameEvents.current.OnLevelEnd -= LoadNextScene;
    }
}