using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridMaker : MonoBehaviour
{
    public static GridMaker instance;

    public NoteSheetEditor sheetEditor;
    public MusicManager MM = MusicManager.instance;
    public GameObject bar_Air;
    public GameObject bar_Road;
    public GameObject bar_Time;
    public List<GameObject> grids = new List<GameObject>();
    public Transform contentContainer;

    public int maxBarGenerateCount = 20;
    public int maxBeatCount = 32;

    public float scrollSpeed = 4f;
    public float snapAmount = 8f;
    public float ScrollSnapAmount
    {
        get { return scrollSpeed; }
        set { scrollSpeed = Mathf.Clamp(value, 1f, 32f); }
    }

    private void Start()
    {
        instance = this;
    }

    public void Init()
    {
        DestroyAll();
        Create();
        //InitPos();
    }

    public void Create()
    {
        maxBarGenerateCount = (int)(((MusicManager.instance.musicBPM / 60f) * MusicManager.instance.musicTotalSec) / 4)+1;
        Debug.Log(String.Format("{0}, {1}, {2}", maxBarGenerateCount, MusicManager.instance.musicBPM, MusicManager.instance.musicTotalSec));

        for (int i = 0; i < maxBarGenerateCount; ++i)
        {
            GameObject ba = Instantiate(bar_Air);
            ba.transform.SetParent(contentContainer);
            ba.transform.localScale = Vector2.one;
            Bar bar_a = ba.GetComponent<Bar>();
            bar_a.barNumber = i+1;
            bar_a.line = DataEnumManager.NoteLine.AIR;

            GameObject br = Instantiate(bar_Road);
            br.transform.SetParent(contentContainer);
            br.transform.localScale = Vector2.one;
            Bar bar_r = br.GetComponent<Bar>();
            bar_r.barNumber = i+1;
            bar_r.line = DataEnumManager.NoteLine.ROAD;

            GameObject bt = Instantiate(bar_Time);
            bt.transform.SetParent(contentContainer);
            bt.transform.localScale = Vector2.one;
            Bar bar_t = bt.GetComponent<Bar>();
            bar_t.barNumber = i+1;

            grids.Add(ba);
            grids.Add(br);
            grids.Add(bt);
        }
    }

    public void DestroyAll()
    {
        for(int i = 0; i < grids.Count; i++)
        {
            if(grids[i] != null)
            {
                GameObject obj = grids[i];
                Destroy(obj);
            }
        }
        grids.Clear();
    }

    public void FindLoadData(NoteData data)
    {
        foreach(var grid in grids)
        {
            if(grid.GetComponent<Bar>() != null)
            {
                if(grid.GetComponent<Bar>().line == data.line)
                {
                    foreach (var beat in grid.GetComponent<Bar>().beats)
                    {
                        if (beat.GetComponent<EditNote>().data.startTime == data.startTime)
                        {
                            beat.GetComponent<EditNote>().OnClick();
                        }
                    }
                }
            }
        }
    }
}
