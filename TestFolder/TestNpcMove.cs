
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestMove
{
    public string name;
    public string direction;
}

public class TestNpcMove : MonoBehaviour
{
    public string direction;

    private OrderManager theOrder;
    // Start is called before the first frame update
    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            theOrder.PreLoadCharacter(); //반드시 호출해야함, 호출하지 않으면 배열이 null값을 가짐
            theOrder.Turn("npc1", direction);
        }
    }
}
