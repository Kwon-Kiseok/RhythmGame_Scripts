using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSheetEditor : MonoBehaviour
{
    public AudioSource audioSource;     // ���� ������ ������� ���� ��ü

    public GameObject selectedObject;   // ���õ� ������Ʈ
    private int currentSelectedLine;        // ���� ���õ� ����
    private int currentSelectedBarNumber;   // ���� ���õ� ����
}
