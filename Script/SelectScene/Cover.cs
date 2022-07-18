using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cover : MonoBehaviour
{
    public MusicData data = new MusicData();
    public bool Selected { get; set; } = false;
    public Image image;

    public void OnSelectSong()
    {
        // ���� ���õȰ� �ƴ϶�� ����
        if(!Selected)
        {
            return;
        }

        var selectMgr = GameObject.FindWithTag("SelectSceneManager");
        selectMgr.GetComponent<SelectSceneManager>().OnClickPlay();
    }
}
