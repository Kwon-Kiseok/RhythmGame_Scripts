using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using DataEnumManager;

public class PlayerController : MonoBehaviour
{
    // spine anim
    public SkeletonAnimation skelAnim;
    public AnimationStateData stateData;
    public AnimationReferenceAsset[] animClips;

    // spine animation state enum
    public enum AnimState
    {
        IN,             // ���� ����
        RUN,            // �޸��� ����
        JUMP,           // ���� ����
        ATTACK,         // ���� ����
    }

    [SerializeField]
    private AnimState animState;

    [SerializeField]
    private string currentAnim;

    private Rigidbody2D rigid;

    // temp entrance time
    private float entranceDelayTime = 0f;
    private bool isEntrance = true;
    
    private float timer = 0f;

    // temp judgement
    private DataEnumManager.Judgement judge;

    // Character LinePos
    public enum Line
    {
        Air,
        Road,
    }

    // Ray
    public Transform trGroundRay;
    public Transform trAirRay;

    private bool isJump = false;
    public float jumpForce = 10f;

    public int CurrentHP { get; set; } = 250;
    public int MaxHP = 250;
    public bool Dead { get; set; } = false;

    public Transform trRoad;
    public Transform trAir;
    private Line currentLine;

    private AudioSource audioSource;

    public float airRayDistance = 5f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        animState = AnimState.IN;
        currentLine = Line.Road;
        entranceDelayTime = animClips[(int)animState].Animation.Duration;
        SetCurrentAnim(animState, false);
    }

    private void FixedUpdate()
    {
        RaycastHit2D hitGround = Physics2D.Raycast(trGroundRay.position, Vector2.down);
        Debug.DrawRay(trGroundRay.position, Vector2.down * hitGround.distance, Color.yellow);

        if(hitGround.collider != null)
        {
            if(hitGround.distance >= 0.2f)
            {
                currentLine = Line.Air;
                isJump = true;
            }
            else
            {
                currentLine = Line.Road;
                isJump = false;
            }
        }

        Debug.DrawRay(trAirRay.position, Vector2.right * airRayDistance, Color.red);
    }

    private void Update()
    {
        CheckHP();
        // ��� �� �浹 ���� �� ��� �ִϸ��̼� ���
        if (Dead)
        {
            timer += Time.deltaTime;

            skelAnim.AnimationName = "Die";
            Destroy(rigid);

            if(timer > 3f)
            {
                GameManager.instance.GameFail = true;
                SceneLoader.LoadScene("ResultScene");
                timer = 0f;
            }
            return;
        }

        // ó�� ���� �ִϸ��̼��� ���� �ĺ��� ��Ʈ���� �����ؾ� ��
        timer += Time.deltaTime;

        if (timer > entranceDelayTime && isEntrance)
        {
            isEntrance = false;
            timer = 0f;
        }
        else if(!isEntrance)
        {
            Attack();

            AddAnimation(AnimState.RUN, true);
            // ���� �⺻ �ִϸ��̼��� run�ִϸ��̼��� ����Ǿ� �־�� ��
            // df = up or jump, jk = down ������ �Ǿ�� ��

            // ĳ������ ������ ���̸� ���ָ鼭 ��Ʈ�� Ž���� ��쿡�� �������� �� ��� ������ ���;� ��

            // ���󿡼� ���� �� ��쿡�� ����, ������ �Ʒ��� �� ��쿡�� �ٿ��� �ִϸ��̼��� ����ǰ�
            // ���� ��ġ���� �Է¹޾��� ���� �ε���, ������ �ִϸ��̼��� ����Ǿ�� ��

            //audioSource.PlayOneShot(GetComponent<PlayerAudio>().PlayHitSound());

            // ��� �ൿ�� ���������� �޸��� �ִϸ��̼����� ���ư��� ��
        }
    }

    private void AsyncAnimation(AnimationReferenceAsset animClip, bool isLoop, float timeScale)
    {
        if(animClip.name.Equals(currentAnim))
        {
            return;
        }
        skelAnim.state.SetAnimation(0, animClip, isLoop).TimeScale = timeScale;
        skelAnim.loop = isLoop;
        skelAnim.timeScale = timeScale;

        currentAnim = animClip.name;
    }

    private void SetCurrentAnim(AnimState animState, bool isLoop)
    {
        AsyncAnimation(animClips[(int)animState], isLoop, 1f);
    }

    private void AddAnimation(AnimState animState, bool isLoop)
    {
        skelAnim.state.AddAnimation(0, animClips[(int)animState], isLoop, skelAnim.state.GetCurrent(0).AnimationEnd);
        currentAnim = animClips[(int)animState].name;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Note"))
        {
            // ���� �߿� ���� ���
            if(isJump)
            {
                skelAnim.AnimationName = "AirHitHurt";
            }
            // ���󿡼� ���� ���
            else
            {
                skelAnim.AnimationName = "Hurt";
            }

            CurrentHP -= 10;
        }
    }

    private void Attack()
    {
        RaycastHit2D hitAirline = Physics2D.Raycast(trAirRay.position, Vector2.right, airRayDistance);

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
            // ���� ���̿� ��Ʈ�� ����ִٸ� ���������� ������ο��� ������ �Ǿ�� ��
            if (isJump)
            {
                if (hitAirline.collider != null)
                {
                    if (hitAirline.collider.CompareTag("Note"))
                    {
                        transform.position = Vector2.MoveTowards(transform.position, trAir.position, jumpForce);
                        isJump = true;

                        // ���⼭ Ķ���극�̼��� ���� ������ �޾ƿͼ� ������ ���� �ִϸ��̼��� ������־�� ��
                        judge = (Judgement)Random.Range((int)Judgement.EARLY, (int)Judgement.COUNT);

                        JudgeAirHit();
                    }
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, trAir.position, jumpForce);
                isJump = true;

                if (hitAirline.collider == null)
                {
                    skelAnim.AnimationName = "Jump";
                }
                else if (hitAirline.collider.CompareTag("Note"))
                {
                    // ���⼭ Ķ���극�̼��� ���� ������ �޾ƿͼ� ������ ���� �ִϸ��̼��� ������־�� ��
                    judge = (Judgement)Random.Range((int)Judgement.EARLY, (int)Judgement.COUNT);

                    if (currentLine == Line.Road)
                    {
                        skelAnim.AnimationName = "Uphit";
                        currentLine = Line.Air;
                    }
                    else if (currentLine == Line.Air)
                    {
                        JudgeAirHit();
                    }
                }
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
            judge = (Judgement)Random.Range((int)Judgement.EARLY, (int)Judgement.COUNT);
            // ���� �߿� ���� ������ ��� �ٷ� ������
            if (isJump)
            {
                transform.position = Vector2.MoveTowards(transform.position, trRoad.position, jumpForce);
                isJump = false;
                if (currentLine == Line.Air)
                {
                    skelAnim.AnimationName = "DownHit";
                    currentLine = Line.Road;
                }
                else if (currentLine == Line.Road)
                {
                    JudgeRoadHit();
                }
            }
            else
            {
                JudgeRoadHit();
            }
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
#elif UNITY_ANDROID
        }
#endif
        }
    }

    private void JudgeAirHit()
    {
        switch (judge)
        {
            case Judgement.EARLY:
                skelAnim.AnimationName = "Hitmiss";
                break;
            case Judgement.GREAT:
                skelAnim.AnimationName = "AirHitGreat" + Random.Range(1, 4);
                break;
            case Judgement.PERFECT:
                skelAnim.AnimationName = "AirHitPerfect" + Random.Range(1, 5);
                break;
        }
    }

    private void JudgeRoadHit()
    {
        switch (judge)
        {
            case Judgement.EARLY:
                skelAnim.AnimationName = "Hitmiss";
                break;
            case Judgement.GREAT:
                skelAnim.AnimationName = "RoadHitGreat" + Random.Range(1, 4);
                break;
            case Judgement.PERFECT:
                skelAnim.AnimationName = "RoadHitPerfect" + Random.Range(1, 5);
                break;
        }
    }

    private void CheckHP()
    {
        if (CurrentHP < 0)
        {
            CurrentHP = 0;
            Dead = true;
        }
        else if(CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP;
        }
    }
}

