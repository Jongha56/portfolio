## portfolio

**목차**
1. 각 Folder 설명
2. 직접 구현한 기능
3. 출처
---

**1. 각 Folder 설명**

* **EnemyFolder : 주로 몬스터에 관련된 스크립트를 모아놨습니다.**

  * BossController.cs : 보스몬스터의 움직임을 랜덤으로 발생시키고, 플레이어가 근처에 있을 시 공격합니다
  * EnemyStat.cs : 몬스터들의 기본 능력치 설정합니다
  * SlimeController.cs : 슬라임의 움직임을 랜덤으로 발생시키고, 플레이어가 근처에 있을 시 공격합니다
* **EventFolder : 주로 게임을 하면서 발생하는 이벤트에 관련된 스크립트를 모아놨습니다.**
  * Choice.cs : 질문과 선택지를 저장하는 커스텀클래스
  * Close_Door.cs : 플레이어가 잠겨있는 문앞에서 z키 클릭했을때 문이잠겨있다는 것을 알려주는 스크립트
  * Dialogue.cs : 캐릭터 대화창과 관련된 커스텀클래스
  * Effect.cs : 플레이어가 몬스터 때릴때 이펙트 효과
  * Ending.cs : 엔딩 스크립트
  * Event1.cs : 플레이어가 표지판 앞에서 space를 눌렀을때 퀘스트 부여 또는 알림 설정 스크립트
  * FloatingText.cs : 아이템을 획득했을 때 어떤 아이템을 획득했는지 text를 띄워줌
  * HuntEnemy.cs : 몬스터를 사냥하는 스크립트
  * ItemPickUp.cs : 아이템 습득 이벤트
  * LightController.cs : 건물안으로 들어간 경우 손전등 이벤트
  * Menu.cs : 메뉴 창
  * MovingObject.cs : 전체적인 움직임을 담당하는 이벤트
  * NeedItemOpen.cs : 필요한 아이템이 있어야 문이 열리는 이벤트
  * NumberSystem.cs : 정답을 맞출경우 상자를 여는 등의 이벤트
  * OkOrCancel.cs : 장비아이템 착용 / 취소 또는 벗기 / 취소 등 상황에 따라 다른 텍스트를 띄워야 하기 때문에 만든 스크립트
  * Potal.cs : 보스룸으로 이동하는 포탈 이벤트
  * SaveNLoad.cs : 세이브 로드 기능
* **ManagerFolder : 게임을 플레이할 때 스크립트마다 중복되는 기능들을 Manager 스크립트로 만들어 모았습니다.**
  * AudioManager.cs : 전체적인 사운드 관리 스크립트
  * BGMManager.cs : 전체적인 bgm 관리 스크립트
  * CameraManager.cs : 전체적인 카메라 관리 스크립트
  * ChoiceManager.cs : 질문 정답 이벤트를 전체적으로 관리하는 스크립트
  * DatabaseManager.cs : 게임 내 필요한 아이템 등의 정보를 저장하는 스크립트
  * DialogueManager.cs : 캐릭터 대화창을 관리하는 스크립트
  * FadeManager.cs : 페이드 인, 아웃 등의 이벤트를 관리하는 스크립트
  * GameManager.cs : save와 load 시 카메라의 기본 세팅등을 관리하는 스크립트
  * NPCManager.cs : NPC의 움직임을 전체적으로 관리하는 스크립트
  * OrderManager.cs : 이벤트 발생 시에 캐릭터의 움직임을 제어하는 스크립트
  * PlayerManager.cs : 플레이어의 전반적인 움직임을 관리하는 스크립트
  * WeatherManager.cs : 날씨효과를 관리하는 스크립트
* **MapFolder : 맵에 관련된 기능을 가진 스크립트를 모았습니다.**
  * Bound.cs : 카메라가 맵의 범위를 넘지 못하게 제어하는 스크립트
  * StartPoint.cs : 플레이어가 맵 이동시 시작할 지점
  * TransferMap.cs : 플레이어가 같은 씬에서의 다른맵으로 이동할 수 있게 해주는 스크립트
  * TransferScene.cs : 플레이어가 다른 씬의 맵으로 이동할 수 있게 해주는 스크립트
* **PlayerFolder : 플레이어와 관련된 기능을 가진 스크립트를 모았습니다.**
  * Equipment.cs : 플레이어의 장비창
  * Inventory.cs : 플레이어의 인벤토리
  * InventorySlot.cs : 플레이어의 인벤토리에 있는 아이템 정보 저장하는 스크립트
  * Item.cs : 플레이어가 사용할 아이템
  * PlayerStat.cs : 플레이어의 스텟
* **TestFolder : 기능이 제대로 작동하는지 확인하기 위한 스크립트를 모았습니다.**
  * TestDialogue.cs : 이벤트 발생 시 대화창이 제대로 발생하는지 확인
  * TestLight.cs : 이벤트 발생 시 손전등이 켜지는지 확인 
  * TestLightOff.cs : 이벤트 발생 시 손전등이 꺼지는지 확인
  * TestNpcMove.cs : 이벤트 발생 시 NPC의 움직임 확인
  * TestNumber.cs : 보물상자의 번호 입력 이벤트가 잘 발생하는지 확인
  * TestQuestion.cs : 질문과 정답 맞추는 이벤트가 잘 발생하는지 확인
  * TestRain.cs : 비 이벤트가 잘 발생하는지 확인
  * TestScript1.cs : BGM이 잘 나오는지 확인
  * TestScript2.cs : BGM의 Fade In / Out 효과가 잘 되는지 확인
  * TestTextDialogue.cs : 이벤트 발생 시 간단한 텍스트 출력과 캐릭터의 움직임 제어 확인
---
**2. 직접 구현한 기능**
 * BossController.cs
 ```c#
 //보스몬스터와 플레이어간의 위치를 비교해서 보스몬스터가 플레이어로부터 어느 위치에 있는지 받아
 //Attaking함수로 보스의 공격모션을 플레이어가 있는 곳을 향할 수 있게 구현하였습니다.
 private void Attacking(string _direction)
 {
     switch (_direction)
     {
         case "UP":
             animator.SetTrigger("AttackUp"); //trigger기때문에 true, false값이 필요없이 실행가능
             break;
         case "DOWN":
             animator.SetTrigger("AttackDown");
             break;
         case "RIGHT":
             animator.SetTrigger("AttackRight");
             break;
         case "LEFT":
             animator.SetTrigger("AttackLeft");
             break;
     }
     StartCoroutine(WaitCoroutine());
 }
 private void BossPossition()
 {
     PlayerPos = PlayerManager.instance.transform.position;

     if (PlayerPos.x > this.transform.position.x)
         Boss_dir = "RIGHT";
     else if (PlayerPos.x < this.transform.position.x)
         Boss_dir = "LEFT";

     if (PlayerPos.y > this.transform.position.y)
         Boss_dir = "UP";
     else if (PlayerPos.y < this.transform.position.y)
         Boss_dir = "DOWN";
 }
 ```
 
 * Close_Door.cs
 ```c#
 //OnTriggerStay2D를 사용해 해당 캐릭터가 지정한 칸 안에 머무르고 있을경우에 실행할수 있게 설정하였고, 그 칸안에서
 //처음 한번만 실행할 수 있게 flag 를 false로 초기화시킨 후 캐릭터가 위를 보고있고, z키를 누를경우 이벤트가 발동해서
 //flag를 true로 바꿔 다시 이벤트가 발동하지 않도록 막고, 문이 잠겨있다는 대화창을 띄우도록 구현하였습니다.
 private void OnTriggerStay2D(Collider2D collision)
 {
     if (!flag && Input.GetKey(KeyCode.Z) && thePlayer.animator.GetFloat("DirY") == 1f)
     {
         flag = true;
         StartCoroutine(EventCoroutine());
     }
 }
 IEnumerator EventCoroutine()
 {
     theOrder.PreLoadCharacter();
     theOrder.NotMove();

     theDM.ShowDialogue(dialogue_1);
     yield return new WaitUntil(() => !theDM.talking); //대화가 끝날때까지 대기하다가 끝나면 밑문장 실행

     theOrder.Move();
 }
 ```
 * NeedItemOpen.cs
 ```c#
 //특정한 장소로 이동하기 위해서 플레이어가 문을열때 그 문을 열 수 있는 ID를 가진 아이템을 보유하고 있는지
 //확인하기 위한 코드입니다. inventoryItemList를 확인해서 특정 아이템을 보유하고 있으면 Exist_Item을 true로
 //바꿔 특정 장소를 이동할 수 있게 구현하였습니다.
 private void OnTriggerStay2D(Collider2D collision)
 {
     if (collision.gameObject.name == "Player")
     {
         for(int i = 0; i < theInven.inventoryItemList.Count; i++)
         {
             if (Need_ItemID == theInven.inventoryItemList[i].itemID)
             {
                 Exist_Item = true;
                 flag = false;
                 break;
             }
         }
         if (!flag && Input.GetKeyDown(KeyCode.Z))
         {
             if (Exist_Item)
             {
                 StartCoroutine(TransferCoroutine());
             }
             else
             {
                 flag = true;
                 StartCoroutine(NotTransferCoroutine());
             }
         }
     }
 }
 ```
 * Potal.cs
 ```c#
 //포탈도 NeedItemOpen과 마찬가지로 특정아이템이 인벤토리 안에 있는지 확인해
 //있는 경우에만 포탈이 열려 보스방으로 들어갈 수 있게 구현하였습니다.
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
 ```
 * TestLightOff.cs
 ```c#
 //SetActive(false), SetActive(true)를 사용해 맵정도의 크기의 손전등 이미지를 게임 오브젝트로사용하여
 //불을 키고 끄는 기능을 구현하였습니다.
 private void OnTriggerEnter2D(Collider2D collision)
 {
     if (flag)
     {
         go.SetActive(false);
     }
 }
 ```
 * Equipment.cs
 ```c#
 //Equipment.cs 에서 아이템을 바꿔 장착할 시 캐릭터의 스텟이 계속 더해져서 오르는 오류가 발생했는데,
 //EquipItemCheck함수에서 장착된 아이템이 있는 경우 TakeOffEffect함수를 사용해 스탯을 감소해줌으로써 오류를 해결하였습니다.
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
```
---
**3. 출처**

[케이디의 유니티강좌 2D 쯔꾸르퐁 게임 만들기](https://www.youtube.com/watch?v=EdsVx9yN2Cc&list=PLUZ5gNInsv_NW8RQx8__0DxiuPZn3VDzP)
