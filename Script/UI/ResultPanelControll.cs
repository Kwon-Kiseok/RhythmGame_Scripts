using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPanelControll : MonoBehaviour
{
    public GameObject[] Fail;
    public GameObject[] Success;

    private void Update()
    {
        if(GameManager.instance.GameClear)
        {
            foreach (var succ in Success)
            {
                succ.SetActive(true);
            }
            foreach (var f in Fail)
            {
                f.SetActive(false);
            }
            GameManager.instance.GameClear = false;
        }
        else if(GameManager.instance.GameFail)
        {
            foreach (var succ in Success)
            {
                succ.SetActive(false);
            }
            foreach (var f in Fail)
            {
                f.SetActive(true);
            }
            GameManager.instance.GameFail = false;
        }
    }
}
