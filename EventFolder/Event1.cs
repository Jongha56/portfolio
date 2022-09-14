using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//플레이어가 표지판 앞에서 space 눌렀을 때 이벤트 발생
public class Event1 : MonoBehaviour
{

    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer; //player가 위(animator.getFlaot "DirY"==1)를 바라보고있을 때 발동하기위해 생성해줌
    private FadeManager theFade;

    private bool flag; //한번만 실행되게 만들어줄 flag 생성

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
    }

    private void OnTriggerStay2D(Collider2D collision) //칸안에 머무르고있을때 실행
    {
        if (!flag && Input.GetKey(KeyCode.Space) && thePlayer.animator.GetFloat("DirY")==1f)
        {
            flag = true;
            StartCoroutine(EventCoroutine());
        }
    }
    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking); //대화가 끝날때까지 대기하다가 끝나면 밑문장 실행

        theOrder.Move("player", "RIGHT");
        theOrder.Move("player", "RIGHT");
        theOrder.Move("player", "UP");
        yield return new WaitUntil(() => thePlayer.queue.Count == 0); //위에서 넣은 움직임이 다 소진되기전까지 대기

        theFade.Flash();
        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDM.talking);



        theOrder.Move();

    }
}
