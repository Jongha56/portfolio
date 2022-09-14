using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNumber : MonoBehaviour
{
    private OrderManager theOrder;
    private NumberSystem theNumber;

    public bool flag;
    public int correctNumber;

    // Start is called before the first frame update
    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theNumber = FindObjectOfType<NumberSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!flag && Input.GetKeyUp(KeyCode.DownArrow))
            StartCoroutine(ACoroutine());
    }
    IEnumerator ACoroutine()
    {
        theOrder.NotMove();
        theNumber.ShowNumber(correctNumber);
        yield return new WaitUntil(() => !theNumber.activated); //패스워드작업이 끝날때까지 대기
        theOrder.Move();

        if (theNumber.GetResult()) //정답이면 더이상 동작안함
            flag = true;
        else //오답일경우 계속 동작
            flag = false;
    }
}
