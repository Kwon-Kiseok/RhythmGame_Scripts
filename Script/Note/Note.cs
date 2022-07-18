using DataEnumManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class NoteData
{
    // ��Ʈ ������ ����
    public string ID;
    [JsonConverter(typeof(StringEnumConverter))]
    public DataEnumManager.NoteType type;
    [JsonConverter(typeof(StringEnumConverter))]
    public DataEnumManager.NoteLine line;
    // startTime�� ��Ȯ�� ������ ��ġ�� �����ؾ� ��
    // endTime�� �ճ�Ʈ�� ���κ��� ��Ȯ�� ������ ��ġ�� �����ؾ� �� (�Ϲ� ��Ʈ�� �ش���� ����)
    public int startTime = 0;
    public float endTime = 0f;
    public float prevStartTime = 0;

    public NoteData()
    {
    }

    public NoteData(string iD, NoteType type, NoteLine line, int startTime, float endTime)
    {
        ID = iD;
        this.type = type;
        this.line = line;
        this.startTime = startTime;
        this.endTime = endTime;
    }
}

public class Note : MonoBehaviour
{
    public Sprite[] sprites;
    public Transform floatingTr;
    public GameObject floatingJudgeObj;

    public ParticleSystem popParticle;
    public float speed;

    public NoteData data;

    private Transform tr;
    private SpriteRenderer ren;

    public bool Pop { get; set; }
    public bool isInJudgement = false;

    private void Awake()
    {
        ren = GetComponent<SpriteRenderer>();
        tr = GetComponent<Transform>();
    }

    private void Start()
    {
        ren.sprite = sprites[Random.Range(0, sprites.Length)];
        speed = 12f;
    }

    private void Update()
    {
        tr.localPosition += speed * Time.smoothDeltaTime * -1 * Vector3.right;
        
        if (isInJudgement)
        {
            if (data.line == DataEnumManager.NoteLine.AIR)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
                {
#elif UNITY_ANDROID
        if(Input.touchCount != 0)
        {
            var touch = Input.GetTouch(0);
            if(touch.position.y > Screen.height/2)
            { 
#endif
                    Hitted();
                }
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
#elif UNITY_ANDROID
        }
#endif
            }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
            {
#elif UNITY_ANDROID
        if(Input.touchCount != 0)
        {
            var touch = Input.GetTouch(0);
            if(touch.position.y < Screen.height/2)
            { 
#endif
                Hitted();
            }
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
#elif UNITY_ANDROID
        }
#endif
        
        }
    }

    // ��Ʈ�� ���Խð��� ����ϰ� ������ Ķ���극�̼����� �ִ� �����??
    // Enter ������ �ð��� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("JudgeLine"))
        {
            //Debug.Log(collision.gameObject.name);
            isInJudgement = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("JudgeLine"))
        {
            isInJudgement = false;

            JudgementManager.instance.NoteMiss();
        }
    }

    public void SetNodeData(NoteData data)
    {
        this.data = data;
    }

    private void Hitted()
    {
        // score add
        GameManager.instance.currentScore += 100;

        GameObject floatingJudge = Instantiate(floatingJudgeObj);
        floatingJudge.transform.position = floatingTr.position;
        floatingJudge.GetComponent<Floating>().judge = DataEnumManager.Judgement.PERFECT;

        NoteManager.instance.PlayHitSound();
        gameObject.SetActive(false);
        JudgementManager.instance.NoteHit(DataEnumManager.Judgement.PERFECT);
    }
}
