using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSheet : MonoBehaviour
{
    [Header("Sheet Info")]
    public string fileName;
    public string imgFileName;
    public int previewTime;
    public float bpm;
    public float offset;

    [Header("Content Info")]
    public string title;
    public string artist;
    public string source;
    public string sheet;
    public string diff;

    [Header("Note Info")]
    public List<int> noteLineAir;
    public List<int> noteLineMid;
    public List<int> noteLineRoad;

    public void Init()
    {
        noteLineAir.Clear();
        noteLineMid.Clear();
        noteLineRoad.Clear();
    }
}
