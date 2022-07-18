using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NoteSpawner : MonoBehaviour
{
    public GameObject noteGO;
    public GameObject destGO;
    public GameObject spawnGO;
    public Transform noteParent;
    public DataEnumManager.NoteLine Line;
    private int bpm = 0;

    private float offsetX = 0f;

    private bool isGenFin = false;

    [SerializeField]
    private Queue<NoteData> noteQueue = new Queue<NoteData>();
    public int noteCount = 0;

    [SerializeField]
    private Transform trAppear = null;

    private void Start()
    {
        offsetX = trAppear.position.x;

        //NoteManager.instance.SetNotedPrevStartTime();

        SetNoteQueueFromData(GameManager.instance.currentData);
        if(MusicManager.instance != null)
            bpm = MusicManager.instance.musicBPM;
    }

    // Update is called once per frame
    void Update()
    {
        if(MusicManager.instance.MusicStart)
        { 
            if(!isGenFin)
            {
                GenerateNotes();
                isGenFin = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Note"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(spawnGO.transform.position, spawnGO.transform.localScale);
        Gizmos.DrawCube(destGO.transform.position, destGO.transform.localScale);
    }

    private void SetNoteQueueFromData(MusicData data)
    {
        foreach(var d in data.notes)
        {
            if(Line == d.line)
            {
                noteQueue.Enqueue(d);
                noteCount++;
            }
        }
    }

    private void GenerateNotes()
    {
        for (int i = 0; i < noteCount; i++)
        {
            var noteData = noteQueue.Dequeue();
            // x값 = 스폰된 위치 * 노트의 도착시간(시작 시간) * 변환 시간
            var spawnNoteObj = Instantiate(noteGO, new Vector3(trAppear.position.x + (noteData.startTime * 0.001f) * offsetX, trAppear.position.y), Quaternion.identity, noteParent);
            spawnNoteObj.GetComponent<Note>().data = noteData;
        }
    }
}
