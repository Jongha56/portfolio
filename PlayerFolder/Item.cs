using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int itemID; //아이템의 고유 ID값, 중복 불가능 (50001, 50002)
    public string itemName; //아이템의 이름, 중복 가능 (고대유물, 고대유물)
    public string itemDescription; //아이템 설명
    public int itemCount; //소지 개수
    public Sprite itemIcon; //아이템의 아이콘
    public ItemType itemType;

    //열거 이외의 다른 값을 받으면 오류가 생김
    public enum ItemType //아이템 타입 열거 (Use, Equip, Quest, ETC)
    {
        Use,
        Equip,
        Quest,
        ETC
    }

    //아이템 속성
    public int atk;
    public int def;
    public int recover_hp;
    public int recover_mp;

    //itemIcon을 인수로 받지 않는 이유는 이미지의 이름을 itemID와 같게 설정하기 때문이다
    public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType, 
                int _atk = 0, int _def = 0, int _recover_hp = 0, int _recover_mp = 0, int _itemCount = 1) //Item 클래스가 호출되면 생성자를 통해서 바로 값을 생성
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDes;
        itemType = _itemType;
        itemCount = _itemCount;
        itemIcon = Resources.Load("ItemIcon/" + _itemID.ToString(), typeof(Sprite)) as Sprite; //명시적 선언을 한 후 as로 캐스팅 해야함

        atk = _atk;
        def = _def;
        recover_hp = _recover_hp;
        recover_mp = _recover_mp;

    }
}
