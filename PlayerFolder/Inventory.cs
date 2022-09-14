using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private DatabaseManager theDatabase; //아이템을 습득해서 인벤토리에 넣어주기 위해서 호출
    private OrderManager theOrder;
    private AudioManager theAudio;
    private OkOrCancel theOOC;
    private Equipment theEquip;

    public string key_sound;
    public string enter_sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound; //퀘스트 아이템 같은경우 소모해도 특정 행동이 없기 때문에 소리만 들려줌

    private InventorySlot[] slots; //인벤토리 슬롯들
    public List<Item> inventoryItemList; //플레이어가 소지한 아이템 리스트
    private List<Item> inventoryTabList; //선택한 탭에따라 다르게 보여질 아이템 리스트

    public Text Description_Text; //부연 설명
    public string[] tabDescription; //탭 부연 설명

    // 부모 객체를 이용해서 자식 객체들을 전부 slots에 넣기 위해 사용
    public Transform tf; //slot의 부모 객체(Grid Slot) 

    public GameObject go; //인벤토리 활성화 비활성화
    public GameObject[] selectedTabImages; //소모품, 장비, 퀘스트, 기타 탭
    public GameObject go_OOC; //선택지 활성화 비활성화
    public GameObject prefab_Floating_Text; //prefabs화 한 floatingtext를 아이템을 습득할때마다 불러오기위해 선언

    private int selectedItem; //선택된 아이템
    private int selectedTab; //선택된 탭

    private int page; //인벤토리 페이지
    private int slotCount; //활성화된 슬롯의 개수
    private const int MAX_SLOTS_COUNT = 12; //최대 슬롯 개수

    private bool activated; //인벤토리 활성화 시 true
    private bool tabActivated; //탭 활성화 시 true
    private bool itemActivated; //아이템창 활성화시 true
    private bool stopKeyInput; //키입력 제한 (아이템 소비할 때 질의가 나올 텐데, 그 때 키입력 방지)
    private bool preventExec; //중복실행 제한

    private int check_Use_Inven = 0; //인벤토리 소모품탭에 아이템 있는지 확인
    private int check_Equip_Inven = 0; //인벤토리 장비탭에 아이템 있는지 확인

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);


    // Start is called before the first frame update
    void Start()
    {
        instance = this; //인스턴스에 자기자신을 넣어줌

        theOrder = FindObjectOfType<OrderManager>();
        theAudio = FindObjectOfType<AudioManager>();
        theDatabase = FindObjectOfType<DatabaseManager>();
        theOOC = FindObjectOfType<OkOrCancel>();
        theEquip = FindObjectOfType<Equipment>();

        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf.GetComponentsInChildren<InventorySlot>(); //tf의 자식객체들이 전부 slots에 들어감

    }

    public List<Item> SaveItem() //인벤토리 아이템들이 private으로 되어있기 때문에 save load할때 불러오지 못해서 함수를 사용해 저장
    {
        return inventoryItemList;
    }

    public void LoatItem(List<Item> _itemList) //세이브한 아이템을 로드할때 불러옴
    {
        inventoryItemList = _itemList;
    }

    public void EquipToInventory(Item _item) //장비창에서 장비 해제 버튼 누를 시 사용할 함수
    {
        inventoryItemList.Add(_item); //인벤토리에 해제한 장비 추가
        check_Equip_Inven++;
    }

    public void GetAnItem(int _itemID, int _count = 1) //아이템 주웠을때
    {
        for(int i = 0; i < theDatabase.itemList.Count; i++) //데이터베이스 아이템 검색
        {
            if (_itemID == theDatabase.itemList[i].itemID) //새로 얻은 아이템이 데이터베이스 아이템리스트에 있는경우
            {
                var clone = Instantiate(prefab_Floating_Text,PlayerManager.instance.transform.position,Quaternion.Euler(Vector3.zero));
                //Instantiate : prefab을 생성시키는 함수 -> clone에 넣음
                //정확한 형식을 모를때 var사용
                clone.GetComponent<FloatingText>().text.text = theDatabase.itemList[i].itemName + " " + _count + "개 획득 +";
                //캔버스 밖에 생성되기 때문에 생성되도 보이지 않음
                clone.transform.SetParent(this.transform); //clone을 인벤토리 안에 자식객체로 생성 -> 캔버스 안에 생성되서 보이게 됨

                for(int j = 0; j < inventoryItemList.Count; j++) //인벤토리아이템리스트를 찾아서
                {
                    if (inventoryItemList[j].itemID == _itemID) //새로얻은 아이템이 이미 인벤토리아이템리스트에 있던 아이템이라면
                    {
                        if (inventoryItemList[j].itemType == Item.ItemType.Use) //소모품일경우
                        {
                            inventoryItemList[j].itemCount += _count; //개수만 증감시켜줌
                            
                        }
                        else //소모품이 아닐경우
                        {
                            inventoryItemList.Add(theDatabase.itemList[i]); //아이템하나 추가해줌

                        }
                        return;
                    }
                }
                inventoryItemList.Add(theDatabase.itemList[i]); //새로얻은 아이템이 인벤토리에 없던 처음 얻는 아이템이면 인벤토리에 아이템 추가
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
                return;
            }
        }
        Debug.LogError("데이터베이스에 해당 ID값을 가진 아이템이 존재하지 않습니다."); //데이터베이스에 아이템ID없음

    }

    public void RemoveSlot() //인벤토리 슬롯 초기화
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false); //이미지 전부 사라지게 만듦
        }
    }

    public void ShowTab() //탭 활성화
    {
        RemoveSlot();
        SelectedTab();
    }
    public void SelectedTab() //선택된 탭창 표시, 부연설명기능
    {
        StopAllCoroutines();
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        color.a = 0f;
        for (int i = 0; i < selectedTabImages.Length; i++)
        {
            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        Description_Text.text = tabDescription[selectedTab]; //선택된 탭의 부연설명을 나타내줌
        StartCoroutine(SelectedTabEffectCoroutine());
    }
    IEnumerator SelectedTabEffectCoroutine() //선택된 탭이 반짝반짝 빛나게 해줄 코루틴
    {
        while (tabActivated)
        {
            Color color = selectedTabImages[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    public void ShowItem()//아이템 활성화(inventoryTabList에 조건에 맞는 아이템들만 넣어주고, 인벤토리 슬롯에 활성화
    {
        inventoryTabList.Clear(); //다음 창으로 넘어갈때 이전 창에 있던 아이템이 다음창으로 같이 넘어가지는 것 방지
        RemoveSlot(); //slot도 같이 초기화
        selectedItem = 0; //처음에는 무조건 첫번째 아이템을 선택하게 설정
        page = 0;

        switch (selectedTab) 
        {
            case 0: //소모품
                for(int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Use == inventoryItemList[i].itemType)
                    {
                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;
            case 1: //장비
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Equip == inventoryItemList[i].itemType)
                    {
                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;
            case 2: //퀘스트
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Quest == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 3: //기타
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.ETC == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
        }//탭에 따른 아이템 분류, 그것을 인벤토리 탭 리스트에 추가

        ShowPage();
        SelectedItem(); //선택된 아이템만 반짝반짝하는 효과 부여

    }

    public void ShowPage()
    {
        slotCount = -1; //slotCount가 0일경우 0번째잇는 아이템을 불러오지 못하는 경우가 생김

        for (int i = page * MAX_SLOTS_COUNT; i < inventoryTabList.Count; i++)
        {
            slotCount = i - (page * MAX_SLOTS_COUNT); //slot의 범위는 0~11까지이기 때문에
            slots[slotCount].gameObject.SetActive(true);
            slots[slotCount].Additem(inventoryTabList[i]);

            if (slotCount == MAX_SLOTS_COUNT - 1)
                break; //인벤토리의 슬롯은 12개밖에 안들어가기때문에 12개의 아이템이 차면 break
        }//인벤토리 탭 리스트의 내용을, 인벤토리 슬롯에 추가
    }

    public void SelectedItem()//선택된 아이템을 제외하고, 다른 모든 탭의 컬러 알파값을 0으로 조정
    {
        StopAllCoroutines();
        if (slotCount > -1)
        {
            Color color = slots[0].selected_Item.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i <= slotCount; i++)
                slots[i].selected_Item.GetComponent<Image>().color = color; //선택된 아이템을 제외한 나머지 아이템을 투명하게
            Description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else //인벤토리에 아이템이 하나도 없을 경우
            Description_Text.text = "해당 타입의 아이템을 소유하고 있지 않습니다.";
    }
    IEnumerator SelectedItemEffectCoroutine() //선택된 아이템이 반짝반짝 빛나게 해줄 코루틴
    {
        while (itemActivated)
        {
            Color color = slots[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color; //패널만 반짝반짝
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color; //패널만 반짝반짝
                yield return waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopKeyInput)
        {
            if (Input.GetKeyDown(KeyCode.I)) //i를 한번 누를때 인벤토리창 활성화 / 비활성화
            {
                activated = !activated;

                if (activated) //활성화
                {
                    theAudio.Play(open_sound);
                    theOrder.NotMove();
                    go.SetActive(true);
                    selectedTab = 0;
                    tabActivated = true;
                    itemActivated = false;
                    ShowTab();
                }
                else //비활성화
                {
                    theAudio.Play(cancel_sound);
                    StopAllCoroutines();
                    go.SetActive(false);
                    tabActivated = false;
                    itemActivated = false;
                    theOrder.Move();
                }
            }

            if (activated) //인벤토리가 활성화된 후
            {
                if (tabActivated) //탭활성화시 키입력처리
                {
                    //오른쪽 방향키 누르면 소모품->장비->퀘스트->기타->소모품 순으로 이동
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedTab < selectedTabImages.Length - 1)
                            selectedTab++;
                        else
                            selectedTab = 0;
                        theAudio.Play(key_sound);
                        SelectedTab(); //모든 코루틴을 멈춘후 다시 시작, 선택된 것만 반짝반짝
                    }
                    //왼쪽 방향키 누르면 소모품<-장비<-퀘스트<-기타<-소모품 순으로 이동
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedTab > 0)
                            selectedTab--;
                        else
                            selectedTab = selectedTabImages.Length - 1;
                        theAudio.Play(key_sound);
                        SelectedTab(); //모든 코루틴을 멈춘후 다시 시작, 선택된 것만 반짝반짝
                    }
                    else if (Input.GetKeyDown(KeyCode.Z)) //선택햇을경우 탭창이 조금 짙어지게 만듦
                    {
                        theAudio.Play(enter_sound);
                        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                        color.a = 0.25f;
                        selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                        itemActivated = true; //아이템창 활성화
                        tabActivated = false; //탭창 비활성화
                        preventExec = true; //중복실행방지 true일때 키입력 방지
                        ShowItem();
                    }
                }
                else if (itemActivated) //아이템창 활성화 시 키입력처리
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {//inventoryTabList.Count는 12이지만 배열은 0부터 시작하기때문에 11만 필요해서 -1을 하지만 slotCount는 그 과정이 필요없음
                        if (selectedItem + 2 > slotCount)
                        {//inventoryTabList.Count는 12이지만 배열은 0부터 시작하기때문에 11만 필요해서 -1을 하지만 slotCount
                            if (page < (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT) //페이지번호가 최대 페이지수보다 작으면
                                page++; //페이지 증가
                            else //페이지번호가 최대페이지와 같아지면
                                page = 0; //페이지 다시 0으로 돌아옴

                            RemoveSlot();
                            ShowPage();
                            selectedItem = -2; //-2를 넣어서 인벤토리슬롯의 끝에서 아래키를 눌러 +2 하면 0번째가 선택되게 해줌
                        }

                        if (selectedItem < slotCount - 1)
                            selectedItem += 2; //아래키를 누르면 2칸씩이동
                        else //마지막 줄까지 이동하면 2로나눈 나머지로 초기화
                            selectedItem %= 2;
                        theAudio.Play(key_sound);
                        SelectedItem();
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        if (selectedItem - 2 < 0)
                        {
                            if (page != 0) //페이지번호가 0이 아니면
                                page--; //페이지 감소
                            else //페이지번호가 0이되면
                                page = (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT; //페이지 다시 마지막 페이지로 감

                            RemoveSlot();
                            ShowPage();
                        }

                        if (selectedItem > 1)
                            selectedItem -= 2; //위키를 누르면 2칸씩이동
                        else 
                            selectedItem = slotCount - selectedItem;
                        theAudio.Play(key_sound);
                        SelectedItem();
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedItem + 1 > slotCount)
                        {
                            if (page < (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT) //페이지번호가 최대 페이지수보다 작으면
                                page++; //페이지 증가
                            else //페이지번호가 최대페이지와 같아지면
                                page = 0; //페이지 다시 0으로 돌아옴

                            RemoveSlot();
                            ShowPage();
                            selectedItem = -1;
                        }

                        if (selectedItem < slotCount)
                            selectedItem++;
                        else
                            selectedItem = 0;
                        theAudio.Play(key_sound);
                        SelectedItem();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedItem - 1 < 0)
                        {
                            if (page != 0) //페이지번호가 0이 아니면
                                page--; //페이지 감소
                            else //페이지번호가 0이되면
                                page = (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT; //페이지 다시 마지막 페이지로 감

                            RemoveSlot();
                            ShowPage();
                        }

                        if (selectedItem > 0)
                            selectedItem--;
                        else
                            selectedItem = slotCount;
                        theAudio.Play(key_sound);
                        SelectedItem();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z)&& !preventExec)
                    {
                     //   Debug.Log("equip " + check_Equip_Inven);
                       // Debug.Log("use " + check_Use_Inven);
                        if (selectedTab == 0) //탭창이 소모품 창일경우
                        {
                            theAudio.Play(enter_sound);
                            stopKeyInput = true; //소비아이템을 소모하기위해 움직임 멈춤

                            //물약을 마실 거냐? 같은 선택지 호출
                            StartCoroutine(OOCCoroutine("사용", "취소"));
                        }
                        else if (selectedTab == 1) //장비장착
                        {
                            theAudio.Play(enter_sound);
                            stopKeyInput = true; //장비아이템을 장착하기위해 움직임 멈춤

                            //장비를 장착할 거냐? 같은 선택지 호출
                            StartCoroutine(OOCCoroutine("장착","취소"));
                        }
                        else //퀘스트, ETC -> beep음 출력
                        {
                            theAudio.Play(beep_sound);
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.X)) //아이템창에서 탭창으로 활성화
                    {
                        theAudio.Play(cancel_sound);
                        StopAllCoroutines(); //모든 코루틴 멈춘후
                        itemActivated = false;
                        tabActivated = true;
                        ShowTab(); //탭 코루틴 실행
                    }
                }

                //탭창에서의 z와 중복되지 않게 하기 위해서 탭창에서 z를 눌렀을때
                //true로 만들어줘서 아이템창에서의 z키를 눌렀을때 중복실행되지 않게 하고
                //z키를 뗏을때 다시 false로 바꿔 아이템창에서 z키를 활성화시킬 수 있게 바꿔줌
                if (Input.GetKeyUp(KeyCode.Z)) //중복실행 방지
                    preventExec = false;
            }
        }
    }

    IEnumerator OOCCoroutine(string _up, string _down) //OK or Cancel 코루틴
    {
        theAudio.Play(enter_sound); //if문마다 중복되게 사용하기 때문에 코루틴에 포함
        stopKeyInput = true; //if문마다 중복되게 사용하기 때문에 코루틴에 포함

        go_OOC.SetActive(true);
        //theOOC.ShowTwoChoice("사용", "취소");
        theOOC.ShowTwoChoice(_up, _down); //사용 취소 , 해제 취소 등 상황에 맞게 다른 버튼을 불러오기 위해 인수를 사용

        yield return new WaitUntil(() => !theOOC.activated); //theOOC의 activated가 false가 될떄까지 대기
        if (theOOC.GetResult()) //사용 버튼을 누를 경우
        {
            for (int i = 0; i < inventoryItemList.Count; i++) //인벤토리아이템을 확인
            {
                if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID) //사용할 아이템이 선택될아이템과 같을경우
                {
                    if (selectedTab == 0) //소모품 탭일경우
                    {
                        theDatabase.UseItem(inventoryItemList[i].itemID); //아이템 소모효과 출력

                        if (inventoryItemList[i].itemCount > 1) //소지개수가 2개이상이면
                            inventoryItemList[i].itemCount--; //1개감소
                        else
                        { //소지개수가 1개이하이면
                            inventoryItemList.RemoveAt(i); //i번째 인벤토리 제거
                        }

                        //theAudio.Play(); //아이템 먹는소리 추가가능
                        ShowItem(); //가진 개수가 줄어들었기 때문에 바뀐수치를 적용하기위해 다시 호출해야함
                        break;
                    }
                    else if (selectedTab == 1) //장비 탭일경우
                    {
                        theEquip.EquipItem(inventoryItemList[i]); //인벤토리에 있는 장착할 아이템을 장비창에 넘겨주고
                        inventoryItemList.RemoveAt(i); //해당 아이템 지움
                        ShowItem();
                        break;
                    }
                }
                
            }
        }
        stopKeyInput = false; //인벤토리 방향키가 다시 움직일수있게 false로 바꿔줌
        go_OOC.SetActive(false);
    }
}