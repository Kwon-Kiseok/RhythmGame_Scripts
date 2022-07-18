using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditNote : MonoBehaviour
{
    public NoteData data = new NoteData();
    private Color originColor;
    public bool isSetNote { get; set; } = false;
    public TextMeshProUGUI timetext;

    private void Start()
    {
        if(timetext == null)
            originColor = GetComponent<Image>().color;
    }

    public void OnClick()
    {
        if(!isSetNote)
        {
            isSetNote = true;
            GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            isSetNote = false;
            GetComponent<Image>().color = originColor;
        }

    }
}
