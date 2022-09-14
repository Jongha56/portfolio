using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public GameObject go;
    public OrderManager theOrder;
    public FadeManager theFade;

    private void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theFade = FindObjectOfType<FadeManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            theFade.FadeOut();
            theOrder.NotMove();
            go.SetActive(true);
        }
    }
}
