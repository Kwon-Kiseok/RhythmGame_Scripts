using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSheetEditor : MonoBehaviour
{
    public AudioSource audioSource;     // 현재 편집할 오디오를 받을 객체

    public GameObject selectedObject;   // 선택된 오브젝트
    private int currentSelectedLine;        // 현재 선택된 라인
    private int currentSelectedBarNumber;   // 현재 선택된 마디
}
