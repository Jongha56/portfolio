using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    private OrderManager theOrder;
    private AudioManager theAudio;
    private PlayerStat thePlayerStat;
    private Inventory theInven;
    private OkOrCancel theOOC;

    public string key_sound;
    public string enter_sound;
    public string open_sound;
    public string close_sound;
    public string takeoff_sound;
    public string beep_sound;
    public string equip_sound;

    //const는 변수를 상수처럼 바꿔주기 때문에 값을 바꿀수없음
    private const int WEAPON = 0, SHIELD = 1, AMULT = 2, LEFT_RING = 3, RIGHT_RING = 4,
                    HELMET = 5, ARMOR = 6, LEFT_GLOVE = 7, RIGHT_GLOVE = 8, BELT = 9,
                    LEFT_BOOTS = 10, RIGHT_BOOTS = 11;

    private const int ATK = 0, DEF = 1, HPR = 6, MPR = 7;

   // public int show_atk, show_def, show_hpr, show_mpr; //플레이어스텟+아이템스텟
    public int added_atk, added_def, added_hpr, added_mpr; //save load 기능 구현할떄 능력치도 저장해둬야 캐릭터의 원래스텟과 추가스텟이 구별됨

    public GameObject equipWeapon; //무기를 장착할때 무기 모양을 가져옴

    public GameObject go; //활성화될때만 장비창 키고 끄기
    public GameObject go_OOC;

    public Text[] text; //스탯
    public Image[] img_slots; //장비 아이콘들
    public GameObject go_selected_Slot_UI; //선택된 장비 슬롯 UI

    public Item[] equipItemList; //장착된 장비 리스트

    private int selectedSlot; //방향키를 눌렀을때 선택된 장비 슬롯의 위치값

    public bool activated = false;
    private bool inputKey = true;


    // Start is called before the first frame update
    void Start()
    {
        theInven = FindObjectOfType<Inventory>();
        theOrder = FindObjectOfType<OrderManager>();
        theAudio = FindObjectOfType<AudioManager>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theOOC = FindObjectOfType<OkOrCancel>();
    }

    public void ShowTxT() //장비창에서 플레이어 스텟 표시
    {
        if (added_atk == 0)
            text[ATK].text = thePlayerStat.atk.ToString();
        else
            text[ATK].text = thePlayerStat.atk.ToString() + "(+" + added_atk + ")";

            //text[ATK].text = show_atk + "(" + thePlayerStat.atk.ToString() + "+" + added_atk + ")";

        if (added_def==0)
            text[DEF].text = thePlayerStat.def.ToString();
        else
            text[DEF].text = thePlayerStat.def.ToString() + "(+" + added_def + ")";

        if(added_hpr==0)
            text[HPR].text = thePlayerStat.recover_hp.ToString();
        else
            text[HPR].text = thePlayerStat.recover_hp.ToString() + "(+" + added_hpr + ")";

        if(added_mpr==0)
            text[MPR].text = thePlayerStat.recover_mp.ToString();
        else
            text[MPR].text = thePlayerStat.recover_mp.ToString() + "(+" + added_mpr + ")";

    }
    public void EquipItem(Item _item)
    {
        string temp = _item.itemID.ToString();
        temp = temp.Substring(0, 3);//아이템아이디 앞에서 3글자잘라서 확인코드로사용
        switch (temp)
        {
            case "200": //무기
                EquipItemCheck(WEAPON, _item);
                equipWeapon.SetActive(true);
                equipWeapon.GetComponent<SpriteRenderer>().sprite = _item.itemIcon;
                break;
            case "201": //방패
                EquipItemCheck(SHIELD, _item);
                break;
            case "202": //아뮬렛
                EquipItemCheck(AMULT, _item);
                break;
            case "203": //왼쪽반지
                EquipItemCheck(LEFT_RING, _item);
                break;
            case "204": //오른쪽 반지
                EquipItemCheck(RIGHT_RING, _item);
                break;
            case "205": //갑옷
                EquipItemCheck(ARMOR, _item);
                break;
        }
    }

    public void EquipItemCheck(int _count, Item _item) //아이템 장착 함수
    {
        if (equipItemList[_count].itemID == 0) //장착된 아이템이 없을경우
        {
            equipItemList[_count] = _item; //장착할 아이템 장착
        }
        else //장착된 아이템이 있는경우
        {
            theInven.EquipToInventory(equipItemList[_count]); //장착하고 있던 아이템을 해제한 후 인벤토리에 넣어주고
            TakeOffEffect(equipItemList[_count]);
            equipItemList[_count] = _item; //장착할 아이템 장착
        }

        EquipEffect(_item); //장착한 아이템의 스텟만큼 캐릭터 스탯증가
        theAudio.Play(equip_sound);
        ShowTxT(); 
    }

    public void SelectedSlot()
    {
        go_selected_Slot_UI.transform.position = img_slots[selectedSlot].transform.position;
    }

    public void ClearEquip()
    {
        Color color = img_slots[0].color;
        color.a = 0f;

        for(int i = 0; i < img_slots.Length; i++)
        {//장비창 0~11번까지의 칸의 색을 투명하게 만들고, 아이템모양을 없애줌
            img_slots[i].sprite = null;
            img_slots[i].color = color;
        }

    }

    public void ShowEquip()
    {
        Color color = img_slots[0].color;
        color.a = 1f;

        for (int i = 0; i < img_slots.Length; i++)
        {
            if (equipItemList[i].itemID != 0) //무언가 장착되어 있으면
            {
                img_slots[i].sprite = equipItemList[i].itemIcon; //해당 아이콘을 장비창에 넣어줌
                img_slots[i].color = color;
            }
        }

    }

    private void EquipEffect(Item _item) //아이템 장착시 스텟증가
    {
        thePlayerStat.atk += _item.atk;
        thePlayerStat.def += _item.def;
        thePlayerStat.recover_hp += _item.recover_hp;
        thePlayerStat.recover_mp += _item.recover_mp;

        added_atk += _item.atk;
        added_def += _item.def;
        added_hpr += _item.recover_hp;
        added_mpr += _item.recover_mp;

    }

    private void TakeOffEffect(Item _item) //아이템 해제시 스텟감소
    {
        thePlayerStat.atk -= _item.atk;
        thePlayerStat.def -= _item.def;
        thePlayerStat.recover_hp -= _item.recover_hp;
        thePlayerStat.recover_mp -= _item.recover_mp;

        added_atk -= _item.atk;
        added_def -= _item.def;
        added_hpr -= _item.recover_hp;
        added_mpr -= _item.recover_mp;
    }


    // Update is called once per frame
    void Update()
    {

        if (inputKey)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {

                activated = !activated;

                if (activated) //장비창 킬 경우
                {
                    theOrder.NotMove();
                    theAudio.Play(open_sound);
                    go.SetActive(true);
                    selectedSlot = 0; //무기슬롯 선택 - 기본값
                    SelectedSlot(); //0번째 슬롯을 기본값으로 설정한 후 적용
                    ClearEquip();
                    ShowEquip();
                    ShowTxT();
                }
                else
                {
                    theOrder.Move();
                    theAudio.Play(close_sound);
                    go.SetActive(false);
                    ClearEquip();
                }
            }

            if (activated) //장비창이 켜져 있을경우
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (selectedSlot < img_slots.Length - 1)
                        selectedSlot++;
                    else
                        selectedSlot = 0;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (selectedSlot < img_slots.Length - 1)
                        selectedSlot++;
                    else
                        selectedSlot = 0;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (selectedSlot > 0)
                        selectedSlot--;
                    else
                        selectedSlot = img_slots.Length - 1;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (selectedSlot > 0)
                        selectedSlot--;
                    else
                        selectedSlot = img_slots.Length - 1;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (equipItemList[selectedSlot].itemID != 0) //장비리스트에서 장착된 아이템이 있는 경우에만 실행
                    {
                        theAudio.Play(enter_sound);
                        inputKey = false;
                        StartCoroutine(OOCCoroutine("벗기", "취소"));
                    }
                    else //장비리스트에서 착용되지 않은 장비슬롯을 누르면 오류 사운드 재생
                    {
                        theAudio.Play(beep_sound);
                    }
                    
                }
            }
        }
    }
    IEnumerator OOCCoroutine(string _up, string _down) //OK or Cancel 코루틴
    {
        go_OOC.SetActive(true);
        theOOC.ShowTwoChoice(_up,_down);
        yield return new WaitUntil(() => !theOOC.activated); //theOOC의 activated가 false가 될떄까지 대기

        if (theOOC.GetResult()) //벗기 버튼(up 버튼)을 누를 경우
        {
            theInven.EquipToInventory(equipItemList[selectedSlot]); //인벤토리에 끼고 있던 아이템 추가
            TakeOffEffect(equipItemList[selectedSlot]); //해제한 아이템의 스텟만큼 캐릭터 스텟 감소

            if (selectedSlot == WEAPON) //무기를 벗을경우 무기 모양 안보이게 만들어줌
                equipWeapon.SetActive(false);
            ShowTxT();

            equipItemList[selectedSlot] = new Item(0, "", "", Item.ItemType.Equip); //초기화
            theAudio.Play(takeoff_sound);
            ClearEquip(); //장비창에 변경된 점이 있기 때문에 초기화 후
            ShowEquip(); //다시 보여줌
        }
        inputKey = true; //장비창 방향키가 다시 움직일수있게 true로 바꿔줌
        go_OOC.SetActive(false);

    }
}
