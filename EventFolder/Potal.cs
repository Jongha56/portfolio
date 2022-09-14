using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject portal;
    public Animator ani_portal;
    private Inventory theInven;
    public int portal_key; //포탈을 열기 위해서 필요한 아이템 아이디

    // Start is called before the first frame update
    void Start()
    {
        theInven = FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < theInven.inventoryItemList.Count; i++)
        {
            if (portal_key == theInven.inventoryItemList[i].itemID)
            {
                portal.SetActive(true);
                ani_portal.SetBool("Open", true);
            }
            else
            {
                portal.SetActive(false);
            }
        }
    }
}
