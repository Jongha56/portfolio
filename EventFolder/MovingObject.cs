using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovingObject : MonoBehaviour
{
    public string characterName;

    public float speed;
    public int walkCount;
    protected int currentWalkCount;

    private bool notCoroutine = false; //npc가 너무 빨리움직이는 것을 방지
    protected Vector3 vector; //캐릭터의 x,y,z좌표

    //npc를 움직일때 size를 늘리면 이동속도가 빨라지고, 스텐딩 모션을 취하지 않는 오류를 해결하기 위해 생성
    public Queue<string> queue;
    //FIFO, 선입선출 자료구조, queue <- a // b // c
    // a,b,c -> 값을 빼면 // b,c // c

    public BoxCollider2D boxCollider; //충돌감지
    //충돌할때 어떤 레이어와 충돌했는지 판단(통과하는 장애물, 통과못하는 장애물 구분)
    public LayerMask layerMask;
    public Animator animator;

    public void Move(string _dir, int _frequency = 5) //함수를 사용할때 frequency의 값을 생략하면 자동으로 5로 넣어준다는 뜻
    {
        queue.Enqueue(_dir);
        if (!notCoroutine) //처음값은 false이기 때문에 무조건 실행
        {
            notCoroutine = true; //true로 바꿔줘서 후에 실행안되게 만듦
            StartCoroutine(MoveCoroutine(_dir, _frequency)); //queue의 모든 값이 빠지고, 다시 flase로 바꿔서 조건문 수행
        }
    }


    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
        while (queue.Count != 0) //queue가 빌때까지 : 전부 dequeue 되면 종료
        {
            switch (_frequency)
            {
                case 1:
                    yield return new WaitForSeconds(4f); //4초대기
                    break;
                case 2:
                    yield return new WaitForSeconds(3f); //3초대기
                    break;
                case 3:
                    yield return new WaitForSeconds(2f); //2초대기
                    break;
                case 4:
                    yield return new WaitForSeconds(1f); //1초대기
                    break;
                case 5:
                    break;
            }

            string direction = queue.Dequeue();
            vector.Set(0, 0, vector.z);

            switch (direction)
            {
                case "UP":
                    vector.y = 1f;
                    break;
                case "DOWN":
                    vector.y = -1f;
                    break;
                case "RIGHT":
                    vector.x = 1f;
                    break;
                case "LEFT":
                    vector.x = -1f;
                    break;
            }

            animator.SetFloat("DirX", vector.x); //들어온 x값을 DirX로 전달해서 실행시킴
            animator.SetFloat("DirY", vector.y);

            //충돌방지기능
            while (true)
            {
                bool checkCollsionFlag = CheckCollsion();
                if (checkCollsionFlag) //방해물을 만나면 이후작업 실행x
                {
                    animator.SetBool("Walking", false);
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    break;
                }
            }
            

            animator.SetBool("Walking", true);

            //speed*walkCount = 48 * 0.7 =33 // 움직이고 싶은 방향으로 미리 33 pixel정도 움직여서
            //npc와 player가 겹치는 일 방지
            boxCollider.offset = new Vector2(vector.x * 0.7f * speed * walkCount, vector.y * 0.7f * speed * walkCount);

            //playermanager와 걷는 방식의 차이 :
            //playermanager는 대각선으로 걷는 입력이 있을 수 있지만
            //movingobject는 x또는 y 둘 중 하나는 반드시 0이기 때문에 대각선으로 걷는 경우가 없음
            // -> vector.Set(0,0,vector.z) 로 둘 중 하나의 값은 반드시 0이됨
            while (currentWalkCount < walkCount)
            {
                //translate : 현재있는 값에서 해당 수치만큼 더해주는 함수
                transform.Translate(vector.x * speed, vector.y * speed, 0);
                currentWalkCount++;

                //walkCount라는 변수를 둔 이유는 각각의 캐릭터, 몬스터마다 이동속도가 다르기 때문에 이동한 위치가 중간을 넘겼을떄 박스컬라이더가 돌아와야하기 때문
                if (currentWalkCount == walkCount * 0.5f + 2) //미리 이동방향으로 움직인 offset 원위치, 
                    boxCollider.offset = Vector2.zero;
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
            if (_frequency != 5) //대기시간 없이 연속적으로 npc가 이동할때 멈추는 모션을 없애줌
                animator.SetBool("Walking", false);
        }
        animator.SetBool("Walking", false); //while문이 끝나면 에니메이션 중단해야함
        notCoroutine = false;
    }

    protected bool CheckCollsion()
    {
        RaycastHit2D hit; //A지점 -> B지점(레이저발사) 
                          // B지점까지 무사히도착 : hit=Null 
                          // 방해물을 만나 B지점까지 도달x : hit=방해물

        //Vector2 start = transform.position; //A지점, 캐릭터의 현재 위치 값
        //Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount); //B지점, 캐릭터가 이동하고자 하는 위치값

        //위의 방식을 사용하면 캐릭터와 npc가 겹쳤을 경우 빠져나오지 못하기 때문에
        //한타일 앞부터 레이져를 쏴서 겹쳐도 빠져나올수 있게 바꿔줌
        Vector2 start = new Vector2(transform.position.x + vector.x * speed * walkCount,
                                    transform.position.y + vector.y * speed * walkCount);//A지점, 캐릭터의 현재 위치 값
        Vector2 end = start + new Vector2(vector.x * speed, vector.y * speed); //B지점, 캐릭터가 이동하고자 하는 위치값


        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider.enabled = true;

        if (hit.transform != null) //방해물을 만나면 이후작업 실행x
            return true; //방해물을 만나면 true 리턴해서 자식 클래스에 알려줌
        return false;
    }
}
