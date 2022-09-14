using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestion : MonoBehaviour
{
    [SerializeField]
    public Choice choice;
    //public Dialogue dialogue1;

    private OrderManager theOrder;
    private ChoiceManager theChoice;

    public bool flag;

    // Start is called before the first frame update
    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!flag)
        {
            StartCoroutine(ACoroutine());
        }
    }
    IEnumerator ACoroutine()
    {
        flag = true;
        theOrder.NotMove();
        theChoice.ShowChoice(choice);
        yield return new WaitUntil(() => !theChoice.choiceIng); //선택지에서 답을 고를때까지 대기
        theOrder.Move();
        Debug.Log(theChoice.GetResult());


    }
}
