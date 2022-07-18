using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    public int timer;

    public GameObject pauseUI;

    public GameObject[] countObjects;
    public GameObject[] readyGoObjects;

    void Start()
    {
        timer = 3;

        foreach (var go in countObjects)
        {
            go.SetActive(false);
        }
        foreach(var go in readyGoObjects)
        {
            go.SetActive(false);
        }
        
        GameManager.instance.GameStop = false;
        GameManager.instance.GameStart = false;

        StartCoroutine(ReadyStart());
    }

    public void OnClickPauseButton()
    {
        if (GameManager.instance.GameStop)
            return;

        GameManager.instance.GameStop = true;
        pauseUI.SetActive(true);
    }

    public void OnClickResumeButton()
    {
        pauseUI.SetActive(false);
        StartCoroutine(CountDownStart());
    }

    public void OnClickReturnButton()
    {
        pauseUI.SetActive(false);
        GameManager.instance.GameStop = false;
        SceneLoader.LoadScene("SelectScene");
    }

    public void OnClickRestartButton()
    {
        pauseUI.SetActive(false);
        GameManager.instance.GameStop = false;
        SceneLoader.LoadScene("GamePlayScene");
    }

    private IEnumerator ReadyStart()
    {
        while (timer > 0)
        {
            readyGoObjects[0].SetActive(true);

            yield return new WaitForSecondsRealtime(1f);

            timer--;
        }

        readyGoObjects[0].SetActive(false);
        readyGoObjects[1].SetActive(true);

        yield return new WaitForSecondsRealtime(1f);

        readyGoObjects[1].SetActive(false);
        GameManager.instance.GameStop = false;
        GameManager.instance.GameStart = true;

        timer = 3;
    }

    private IEnumerator CountDownStart()
    {
        while(timer > 0)
        {
            foreach (var go in countObjects)
            {
                go.SetActive(false);
            }

            countObjects[timer-1].SetActive(true);

            yield return new WaitForSecondsRealtime(1f);

            timer--;
        }

        yield return new WaitForSecondsRealtime(1f);

        foreach (var go in countObjects)
        {
            go.SetActive(false);
        }

        GameManager.instance.GameStop = false;
        GameManager.instance.GameStart = true;

        timer = 3;
    }
}
