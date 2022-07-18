using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneComp : MonoBehaviour
{
    public string nextSceneName;

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            SceneLoader.LoadScene(nextSceneName);
        }
    }
}
