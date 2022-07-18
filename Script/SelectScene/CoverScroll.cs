using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoverScroll : MonoBehaviour
{
    public GameObject scrollbar;
    public MusicPanelController controller;

    float scroll_pos = 0;
    float[] pos;
    Scrollbar scroll;
    int currentIndex = 0;
    bool selectedBtn = false;
    // Start is called before the first frame update
    void Start()
    {
        scroll = scrollbar.GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        pos = new float[transform.childCount];
        float distacne = 1f / (pos.Length - 1);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distacne * i;
        }
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scroll.value;
        }
        else
        {
            if (!selectedBtn)
            {
                for (int i = 0; i < pos.Length; i++)
                {
                    if (scroll_pos < pos[i] + (distacne / 2) && scroll_pos > pos[i] - (distacne / 2))
                    {
                        currentIndex = i;
                        scroll.value = Mathf.Lerp(scroll.value, pos[i], 0.1f);
                    }
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distacne / 2) && scroll_pos > pos[i] - (distacne / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);

                if (!transform.GetChild(i).GetComponent<Cover>().Selected)
                {
                    GameManager.instance.currentData = transform.GetChild(i).GetComponent<Cover>().data;
                    controller.SongTitleText.text = GameManager.instance.currentData.musicName;
                    controller.SongArtistText.text = GameManager.instance.currentData.artist;
                    controller.DemoSet(GameManager.instance.currentData.demoPath);

                    transform.GetChild(i).GetComponent<Cover>().Selected = true;
                }

                for (int j = 0; j < pos.Length; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        transform.GetChild(j).GetComponent<Cover>().Selected = false;
                    }
                }
            }
        }
    }

    public void OnClickRight()
    {
        float distacne = 1f / (pos.Length - 1);
        currentIndex+=1;
        if(currentIndex >= pos.Length)
        {
            currentIndex = pos.Length - 1;
        }
        StartCoroutine(selectBtn(currentIndex * distacne));
    }

    public void OnClickLeft()
    {
        float distacne = 1f / (pos.Length - 1);
        currentIndex-=1;
        if (currentIndex < 0)
        {
            currentIndex = 0;
        }
        StartCoroutine(selectBtn(currentIndex * distacne));
    }


    IEnumerator selectBtn(float targetValue)
    {
        selectedBtn = true;
        while (true)
        {
            yield return null;
            scroll.value = Mathf.Lerp(scroll.value, targetValue, 0.1f);
            if (Mathf.Abs(scroll.value - targetValue) <= 0.1f)
            {
                scroll_pos = scroll.value;
                selectedBtn = false;
                break;
            }
        }
    }
}