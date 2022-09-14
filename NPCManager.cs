using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCMove //npc의 움직임을 담당하는 커스텀 클래스
{
    [Tooltip("NPCMove를 체크하면 NPC가 움직임")]
    public bool NPCmove;
    public string[] direction; //npc가 움직일 방향 설정

    //인스펙터에서 frequency의 스크롤바를 달아줌 / 기능설명적어주기
    [Range(1,5)] [Tooltip("1 = 천천히, 2 = 조금 천천히, 3 = 보통, 4 = 빠르게, 5 = 연속적으로")]
    public int frequency; //npc가 움직일 방향으로 얼마나 빠른 속도로 움직일 것인가
}

public class NPCManager : MovingObject
{
    [SerializeField]
    public NPCMove npc;
    // Start is called before the first frame update
    void Start()
    {
        queue = new Queue<string>();
        StartCoroutine(MoveCoroutine());
    }

    public void SetMove()
    {
    }

    public void SetNotMove()
    {
        StopAllCoroutines();
    }

    IEnumerator MoveCoroutine()
    {
        if (npc.direction.Length != 0)
        {
            for (int i = 0; i < npc.direction.Length; i++)
            {
                /*
                yield return new WaitUntil(() => npcCanMove);//npcCanMove가 true가 될때까지 무한히 대기하는 명령어
                //-> 즉, MovingObject에서 coroutine이 시작할때는 false기 때문에 대기하다가
                //끝날때 true로 바뀌면 여기부터 실행시키기 위해서 사용
                */
                yield return new WaitUntil(() => queue.Count < 2); //npcCanMove로 

                //실질적인 이동 구간
                base.Move(npc.direction[i],npc.frequency);

                if (i == npc.direction.Length - 1)
                    i = -1; // i가 -1이 되면 위의 for문에서 다시 0이 되어서 무한 반복함

            }
        }
    }
}
