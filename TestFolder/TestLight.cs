using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//gameobject를 한번만 실행시키고 싶을때
public class TestLight : MonoBehaviour
{
    public GameObject go;

    private bool flag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!flag)
        {
            go.SetActive(true);
        }
    }
}
