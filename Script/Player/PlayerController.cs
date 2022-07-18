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
        IN,             // 입장 상태
        RUN,            // 달리기 상태
        JUMP,           // 점프 상태
        ATTACK,         // 공격 상태
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
        // 사망 시 충돌 무시 및 사망 애니메이션 재생
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

        // 처음 입장 애니메이션이 끝난 후부터 컨트롤이 가능해야 함
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
            // 제일 기본 애니메이션은 run애니메이션이 적용되어 있어야 함
            // df = up or jump, jk = down 공격이 되어야 함

            // 캐릭터의 앞으로 레이를 쏴주면서 노트가 탐지될 경우에는 점프했을 때 상단 공격이 나와야 함

            // 지상에서 위로 갈 경우에는 업힛, 위에서 아래로 갈 경우에는 다운힛 애니메이션이 재생되고
            // 같은 위치에서 입력받았을 때에 로드힛, 에어힛 애니메이션이 재생되어야 함

            //audioSource.PlayOneShot(GetComponent<PlayerAudio>().PlayHitSound());

            // 모든 행동이 끝마쳐지고 달리기 애니메이션으로 돌아가야 함
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
            // 점프 중에 맞은 경우
            if(isJump)
            {
                skelAnim.AnimationName = "AirHitHurt";
            }
            // 지상에서 맞은 경우
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
            // 만약 레이에 노트가 닿아있다면 연속적으로 에어라인에서 공격이 되어야 함
            if (isJump)
            {
                if (hitAirline.collider != null)
                {
                    if (hitAirline.collider.CompareTag("Note"))
                    {
                        transform.position = Vector2.MoveTowards(transform.position, trAir.position, jumpForce);
                        isJump = true;

                        // 여기서 캘리브레이션을 통한 판정을 받아와서 판정에 따라 애니메이션을 출력해주어야 함
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
                    // 여기서 캘리브레이션을 통한 판정을 받아와서 판정에 따라 애니메이션을 출력해주어야 함
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
            // 점프 중에 지상 공격할 경우 바로 내려옴
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

