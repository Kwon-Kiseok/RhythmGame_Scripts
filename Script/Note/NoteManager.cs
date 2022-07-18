using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    // � ���� ��Ʈ�� �������ִ� �Ŵ��� Ŭ����
    // ����������� ���� �޾ƿ� �� ������ ����
    // ��Ʈ �´� ���ο� �������ش�.
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
