using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    // 곡에 따른 노트를 생성해주는 매니저 클래스
    // 파일입출력을 통해 받아온 곡 정보를 토대로
    // 노트 맞는 라인에 생성해준다.
    public static NoteManager instance;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        instance = this;
    }

    public void SetNotedPrevStartTime()
    {
        var datas = GameManager.instance.currentData;

        for (int i = 0; i < datas.notes.Count; i++)
        {
            if (i == 0)
            {
                datas.notes[i].prevStartTime = 0;
            }
            else if (i != 0)
            {
                var note = datas.notes[i];
                datas.notes[i].prevStartTime = datas.notes[i - 1].startTime;
            }
        }
    }

    public void PlayHitSound()
    {
        audioSource.PlayOneShot(GetComponent<NoteHitAudio>().PlayHitSound());
    }
}
