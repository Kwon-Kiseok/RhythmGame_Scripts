using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditManager : MonoBehaviour
{
    public Scrollbar audioProgressScrollBar;
    public TMP_InputField searchText;
    public GameObject applyButton;
    
    public static EditManager instance;

    private float progressBar_maxValue;

    private bool setMusic = false;
    
    public bool isClickHandle = false;

    private void Start()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        if (!MusicManager.instance.MusicStart)
        {
            audioProgressScrollBar.value = 0;
        }

        if (MusicManager.instance.audioSource.clip != null && MusicManager.instance.MusicStart)
        {
            MusicProgress();   
        }
    }

    private void Update()
    {
        if(!setMusic && MusicManager.instance.audioSource.clip != null)
        {
            setMusic = true;

            progressBar_maxValue = MusicManager.instance.audioSource.clip.length;
        }
    }

    private void MusicProgress()
    {
        if(isClickHandle)
        {
            return;
        }

        var currTime = MusicManager.instance.audioSource.time;

        audioProgressScrollBar.value = currTime / progressBar_maxValue;
    }

    public void ClickHandle()
    {
        if (!isClickHandle)
        {
            isClickHandle = true;
        }
    }

    public void DropHandle()
    {
        if (isClickHandle)
        {
            isClickHandle = false;
        }
    }

    public void ChangeTime()
    {
        if(!isClickHandle)
        {
            return;
        }
        if(MusicManager.instance.audioSource.clip == null)
        {
            return;
        }
        MusicManager.instance.audioSource.time = audioProgressScrollBar.value * MusicManager.instance.audioSource.clip.length;
    }
}
