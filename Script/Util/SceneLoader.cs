using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static string loadSceneName;
    private float timer;
    public float waitingTime = 5f;

    private void Start()
    {
        StartCoroutine(Load());
    }

    public static void LoadScene(string sceneName)
    {
        loadSceneName = sceneName;
        SceneManager.LoadScene("LoadScene");
    }

    private IEnumerator Load()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        while(!op.isDone)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;

            if(op.progress >= 0.9f && timer > waitingTime)
            {
                op.allowSceneActivation = true;
                yield break;
            }
        }
    }
}
