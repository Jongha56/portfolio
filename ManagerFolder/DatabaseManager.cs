using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    static public DatabaseManager instance;
    //필요한 이유 : 1.씬이동 A -> B -> A로 씬 이동시 A에서 발생한 이벤트가 다시 발동되는 것을 막을 수 있음
    //-> database의 어떤 변수의 값이 true이면 다시 이벤트를 발생 안시켜도 되게 만들 수 있음 (전역변수)
    //2. 세이브와 로드 : 데이터베이스에 모든 정보가 저장되어있기 때문에 세이브와 로드를 할 수 있음
    //3. 미리 만들어두면 편하다! ex) 아이템
    // 등등 만들어두면 많이 편리함

    private PlayerStat thePlayerStat;

    //아이템을 사용했을때 텍스트를 띄우기위해 선언
    public GameObject prefabs_Floating_Text;
    public GameObject parent; //canvas

    private void Awake() //싱글턴 화 함수
    {
        //처음 실행될 때는 카메라를 파괴하지 않고
        //다음 실행부터 카메라가 중복생성되는 것을 방지하기 위해서 파괴해줌
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches; //ex) 25번째 스위치 (boss_appear) true. // 세이브하면 다시 false로 안바뀜

    public List<Item> itemList = new List<Item>();

    private void FloatText(int _number,string _color)
    {
        Vector3 vector = thePlayerStat.transform.position; //데미지 받은 텍스트 띄워주기위한 좌표값
        vector.y += 60; //캐릭터의 y값보다 60위에

        GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingText>().text.text = _number.ToString();
        if(_color=="GREEN")
            clone.GetComponent<FloatingText>().text.color = Color.green;
        else if(_color=="BLUE")
            clone.GetComponent<FloatingText>().text.color = Color.blue;

        clone.GetComponent<FloatingText>().text.fontSize = 25;
        clone.transform.SetParent(parent.transform);
    }

    public void UseItem(int _itemID)
    {
        switch (_itemID)
        {
            case 10001:
                if (thePlayerStat.hp >= thePlayerStat.currentHP + 50) //포션을 먹은 후 현재hp가 캐릭터의 hp보다 작거나 같을떄
                    thePlayerStat.currentHP += 50;
                else //포션을 먹은 후 hp가 캐릭터의 hp보다 클때
                    thePlayerStat.currentHP = thePlayerStat.hp;
                FloatText(50, "GREEN");
                //Debug.Log("hp가 50 회복되었습니다.");
                break;
            case 10002:
                if (thePlayerStat.mp >= thePlayerStat.currentMP + 15) //포션을 먹은 후 현재hp가 캐릭터의 hp보다 작거나 같을떄
                    thePlayerStat.currentMP += 15;
                else //포션을 먹은 후 hp가 캐릭터의 hp보다 클때
                    thePlayerStat.currentMP = thePlayerStat.mp;
                FloatText(15, "BLUE");
                //Debug.Log("mp가 15 회복되었습니다.");
                break;

        }
    }

    // Start is called before the first frame update
    void Start()
    {//Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType, 
       //   int _atk = 0, int _def = 0, int _recover_hp = 0, int _recover_mp = 0, int _itemCount = 1)
        thePlayerStat = FindObjectOfType<PlayerStat>();
        itemList.Add(new Item(10001,"빨간 포션","체력을 50 채워주는 마법의 물약", Item.ItemType.Use));
        itemList.Add(new Item(10002, "파란 포션", "마나를 15 채워주는 마법의 물약", Item.ItemType.Use));
        itemList.Add(new Item(10003, "농축 빨간 포션", "체력을 350 채워주는 기적의 농출 물약", Item.ItemType.Use));
        itemList.Add(new Item(10004, "농출 파란 포션", "마나를 80 채워주는 기적의 농축 물약", Item.ItemType.Use));
        itemList.Add(new Item(11001, "랜덤 상자", "랜덤으로 포션이 나온다. 낮은 확률로 꽝", Item.ItemType.Use));
        itemList.Add(new Item(20001, "짧은 검", "기본적인 용사의 검", Item.ItemType.Equip, 3));
        itemList.Add(new Item(20002, "용사의 도끼", "강력한 용사의 도끼이다.", Item.ItemType.Equip, 12));
        itemList.Add(new Item(20003, "녹슨 창", "기본적인 용사의 창", Item.ItemType.Equip, 5));
        itemList.Add(new Item(20301, "사파이어 반지", "1초에 hp 1을 회복시켜주는 마법 반지", Item.ItemType.Equip, 0, 0, 1));
        itemList.Add(new Item(20401, "루비 반지", "1초에 hp 1을 회복시켜주는 마법 반지", Item.ItemType.Equip, 0, 0, 0, 1));
        itemList.Add(new Item(20501, "용사의 갑옷", "강력한 용사의 갑옷이다", Item.ItemType.Equip, 2, 5));
        itemList.Add(new Item(30001, "교무실 열쇠", "교무실의 문을 열 수 있는 열쇠이다.", Item.ItemType.ETC));
        itemList.Add(new Item(40001, "보스방 입장권", "보스방에 들어갈 수 있게 해주는 입장권이다", Item.ItemType.ETC));
    }
}