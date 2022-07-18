using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSceneManager : MonoBehaviour
{
    public SerializeDicPanels panels = new SerializeDicPanels();

    [SerializeField]
    private string prevState;
    [SerializeField]
    private string currState;

    private void Start()
    {
        prevState = "Entrance"; // ���� ó�� �������� ��
        currState = "Enter";
        Init();
    }

    private void Update()
    {
        CheckCurrentPanel();
    }

    private void Init()
    {
        foreach(var panel in panels.Values)
        {
            panel.SetActive(false);
        }
        // ���� ó�� �������� �� ĳ���� ����â
        panels["Enter"].SetActive(true);
    }

    // Enter Button�� ������ �г��� ������ ��
    public void OnClickEnterButton()
    {
        if(panels["Enter"] != null && panels["Enter"].activeSelf)
        {
            panels["Enter"].SetActive(false);
            panels["Music"].SetActive(true);
        }
    }

    public void OnClickBackButton()
    {
        Debug.Log("Clicked back button");

        if(prevState == "Entrance")
        {
            SceneLoader.LoadScene("TitleScene");
            return;
        }

        // ���⼭ ���� �����ִ� UI�� ���� �� ��ư�� �ٸ��� �����ؾ� ��
        if (!panels[prevState].activeSelf)
        {
            panels[currState].SetActive(false);
            panels[prevState].SetActive(true);
        }

    }

    public void OnClickSettingButton()
    {
        Debug.Log("Clicked Setting button");

        if(!panels["Setting"].activeSelf)
        {
            prevState = currState;
            panels[currState].SetActive(false);
            panels["Setting"].SetActive(true);
        }
    }

    private void CheckCurrentPanel()
    {
        if(panels["Enter"].activeSelf && prevState != "Entrance")
        {
            prevState = "Entrance";
            currState = "Enter";
        }
        else if(panels["Music"].activeSelf && prevState != "Enter")
        {
            prevState = "Enter";
            currState = "Music";
        }
        else if(panels["Setting"].activeSelf)
        {
            currState = "Setting";
        }
    }

    public void OnClickEditorButton()
    {
        SceneLoader.LoadScene("EditorScene");
    }

    public void OnClickPlay()
    {
        GameManager.instance.currentScore = 0;
        SceneLoader.LoadScene("GamePlayScene");
    }
}
