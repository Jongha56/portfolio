using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//아이템 습득 이벤트
public class ItemPickUp : MonoBehaviour
{
    public int itemID; //데이터베이스에 넣엇던 아이템ID를 참조하기위한 변수
    public int _count;
    public string pickUpSound;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.instance.Play(pickUpSound); //AudioManager에서 instance를 public으로 선언했기 때문에 어디서든 참조가능
            //-> 한번만 쓰고 잘 안쓰는 경우 변수로 따로 만들지 않고 이런식으로 쓰는게 간편

            //인벤토리에 추가
            Inventory.instance.GetAnItem(itemID, _count);
            Destroy(this.gameObject); //아이템 습득을 하였으니 파괴해줌
        }
    }
}