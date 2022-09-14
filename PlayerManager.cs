using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MovingObject
{
    static public PlayerManager instance;
    public string currentMapName; //transferMap 스크립트에 있는 transferMapName 변수의 값을 저장
    public string currentSceneName; //save load 기능 구현할때 캐릭터가 현재 어느 씬에있는지 알아야함

    public string walkSound_1;
    public string walkSound_2;
    public string walkSound_3;
    public string walkSound_4;

    private AudioManager theAudio;

    //Shift키를 누르면 달릴수 있게 하는 변수
    public float runSpeed;
    private float applyRunSpeed; //실제로 적용되는 값
    private bool applyRunFlag = false;
    private bool canMove = true;
    public bool transferMap = true;

    public bool notMove = false;

    //플레이어 공격
    private bool attacking = false;
    public float attackDelay; //플레이어 공격 딜레이
    private float currentAttackDelay;

    private void Awake()
    {
        //캐릭터가 맵을 이동할때마다 자신이 복제가 되기 때문에 조건문을 달아서 
        //처음맵을 이동할때만 파괴시키지 않고 그 뒤부터는 파괴시키게 만듦
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject); //다른 씬으로 갈 때마다 게임오브젝트 파괴방지 
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        queue = new Queue<string>();
        boxCollider = GetComponent<BoxCollider2D>();
        theAudio = FindObjectOfType<AudioManager>();

        //처음에 에니메이터를 생성했을때 animator 에 Animator 컴포넌트를 넣어줌
        animator = GetComponent<Animator>();
    }

    //update 함수랑 코루틴이 동시에 실행 / 다중처리기능과 비슷하게 보이는 효과
    IEnumerator MoveCoroutine()
    {
        //코루틴은 한번만 실행되고, 그 한번 안에서 반복문이 계속 실행됨
        while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0 && !notMove && !attacking)
        {
            if (Input.GetKey(KeyCode.LeftShift)) //왼쪽 shift키가 눌리면 runspeed를 줘서 달리게만듦
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
            }

            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            //vector.x값이 1일경우 오른쪽이동하고, y값은 0으로 만들어줌
            //좌우로 움직일때 위아래로는 안움직이게 만들어주는 식
            if (vector.x != 0)
                vector.y = 0;


            animator.SetFloat("DirX", vector.x); //들어온 x값을 DirX로 전달해서 실행시킴
            animator.SetFloat("DirY", vector.y);

            bool checkCollsionFlag = base.CheckCollsion();
            if (checkCollsionFlag) //방해물을 만나면 이후작업 실행x
                break; //방해물이 없을경우 계속

            animator.SetBool("Walking", true); //상태전이 : 걷기모션 실행

            int temp = Random.Range(1, 3); //1번, 2번 소리 랜덤하게 재생
            switch (temp)
            {
                case 1:
                    theAudio.Play(walkSound_1);
                    break;
                case 2:
                    theAudio.Play(walkSound_2);
                    break;
                case 3:
                    theAudio.Play(walkSound_3);
                    break;
                case 4:
                    theAudio.Play(walkSound_4);
                    break;
            }

            boxCollider.offset = new Vector2(vector.x * 0.7f * speed * walkCount, vector.y * 0.7f * speed * walkCount);

            while (currentWalkCount < walkCount)
            {
                transform.Translate(vector.x * (speed + applyRunSpeed), vector.y * (speed + applyRunSpeed), 0);
                /*
                //좌,우로만 움직임
                if (vector.x != 0)
                {
                    //translate : 현재있는 값에서 해당 수치만큼 더해주는 함수
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                }
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }
                */
                if (applyRunFlag) //이 함수가 없을 경우 뛸떄 2칸씩 이동함
                    currentWalkCount++; // 뛸경우 currentWalkCount를 2씩증가 -> 한칸이동, 속도두배
                if (currentWalkCount == 12) //미리 이동방향으로 움직인 offset 원위치
                    boxCollider.offset = Vector2.zero;

                currentWalkCount++; //걸어다닐경우 currentWalkCount를 1씩증가
                yield return new WaitForSeconds(0.01f); //0.01초 대기하는 코루틴

            }
            currentWalkCount = 0;

        }
        animator.SetBool("Walking", false); //상태전이 : 서있는 모션 실행
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (canMove && !notMove && !attacking)
        {
            //우 방향키가 눌리면 1리턴, 좌 방향키가 눌리면 -1 리턴
            //위 방향키가 눌리면 1리턴, 아래 방향키가 눌리면 -1 리턴
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false; //자동반복방지
                StartCoroutine(MoveCoroutine());
            }
        }

        if(!notMove && !attacking)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentAttackDelay = attackDelay;
                attacking = true;
                animator.SetBool("Attacking", true);
            }
        }

        if (attacking) //한번공격하면 다음공격까지 딜레이걸어줌
        {
            currentAttackDelay -= Time.deltaTime;
            if (currentAttackDelay <= 0)
            {
                animator.SetBool("Attacking", false);
                attacking = false;
            }
        }
    }
}
