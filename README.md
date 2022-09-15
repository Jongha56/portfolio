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

  
  
  
  




