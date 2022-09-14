using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//인벤토리슬롯의 아이템 이름, 개수, 아이템의 아이콘을 바꾸는 스크립트
public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Text itemName_Text;
    public Text itemCount_Text;
    public GameObject selected_Item; //선택됐을 때 시각적인 효과 부여해주기 위해 사용

    public void Additem(Item _item)
    {
        itemName_Text.text = _item.itemName;
        icon.sprite = _item.itemIcon;
        if (Item.ItemType.Use == _item.itemType) //소모품일 경우에만 중복된 숫자 표기
        {
            if (_item.itemCount > 0)
                itemCount_Text.text = "x " + _item.itemCount.ToString();
            else
                itemCount_Text.text = "";
        }
    }
    public void RemoveItem()
    {
        itemName_Text.text = "";
        itemCount_Text.text = "";
        icon.sprite = null;
        
    }
}
