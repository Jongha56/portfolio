using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLightOff : MonoBehaviour
{
    public GameObject go;

    private bool flag = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (flag)
        {
            go.SetActive(false);
        }
    }
}