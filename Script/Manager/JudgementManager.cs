using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementManager : MonoBehaviour
{
    public static JudgementManager instance;

    public void Start()
    {
        instance = this;
    }

    public void NoteHit(DataEnumManager.Judgement judge)
    {
        // ���⼭ ����ó���� ���� ��
        //Debug.Log("Note Hit " + judge.ToString());
    }

    public void NoteMiss()
    {
        // ������ ��
        //Debug.Log("Note Miss");
    }
}
