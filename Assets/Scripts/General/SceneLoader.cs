using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public static class SceneManagerEvents
{
    public static event Action<string> SceneLoadRequested;
    public static event Action<float> SceneLoadProgress;
    public static event Action SceneLoadStarted;
    public static event Action SceneLoadCompleted;

    public static void RequestSceneLoad(string sceneName) => SceneLoadRequested?.Invoke(sceneName);
    public static void SendProgress(float progress) => SceneLoadProgress?.Invoke(progress);
    public static void SendStarted() => SceneLoadStarted?.Invoke();
    public static void SendCompleted() => SceneLoadCompleted?.Invoke();
}

public class SceneLoader : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManagerEvents.SceneLoadRequested += LoadScene;
    }

    private void OnDisable()
    {
        SceneManagerEvents.SceneLoadRequested -= LoadScene;
    }

    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        SceneManagerEvents.SendStarted();

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            SceneManagerEvents.SendProgress(progress);

            if (op.progress >= 0.9f)
                op.allowSceneActivation = true;

            yield return null;
        }

        SceneManagerEvents.SendCompleted();
    }
}