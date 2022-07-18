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
        // 여기서 판정처리를 해줄 것
        //Debug.Log("Note Hit " + judge.ToString());
    }

    public void NoteMiss()
    {
        // 놓쳤을 때
        //Debug.Log("Note Miss");
    }
}
