using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임을 끄면 public class들도 모두 사라지기 떄문에 외부파일을 만들어서 저장해야함
//게임도중 저장버튼을 누르면 물리적인 파일을 Asset폴더에 생성시키기 위한 라이브러리를 불러옴
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; //직렬화 된 것을 이진파일로만드는 형식 변환기
using UnityEngine.SceneManagement; //씬전환

public class SaveNLoad : MonoBehaviour
{
    //인스펙터 테이블에서 노출시키기 위한 명령어, MonoBehaviour의 상속을 받기 때문에
    //커스텀클래스는 기본적으로 노출이 되지않음, 클래스 앞에 써줌으로써 클래스묶음으로
    //인스펙터창에 노출을 시킬 수 있음
    [System.Serializable] //-> 세이브와 로드기능에서 필수적인 속성(직렬화 ex) 0101011010 : 컴퓨터 입장에서 읽고쓰기 편해짐)
    public class Data //세이브한 정보들 저장해둘곳 -> 모든 수치를 데이터화 시켜서 기록
    {
        //몬스터의 정보도 저장하고 싶으면 따로 만들어주면 됨

        public float playerX; //vector3는 직렬화가 불가능하기 때문에 각각 위치값들을 저장
        public float playerY;
        public float playerZ;
        
        public int playerLv;
        public int playerHP;
        public int playerMP;

        public int playerCurrentHP;
        public int playerCurrentMP;
        public int playerCurrentEXP;

        public int playerHPR;
        public int playerMPR;

        public int playerATK;
        public int playerDEF;

        public int added_atk;
        public int added_def;
        public int added_hpr;
        public int added_mpr;

        //int -> 아이템의 ID값으로 저장가능
        public List<int> playerItemInventory; //Item은 직접 만든 클래스이기 때문에 직렬화가 안됨
        public List<int> playerItemInventoryCount; //아이템을 몇개 가지고 있는지
        public List<int> playerEquipItem; //플레이어가 장착한 아이템의 ID값

        public string mapName; //캐릭터가 있던 맵이름
        public string sceneName; //캐릭터가 있던 씬이름

        public List<bool> swList; // DatabaseManager에 저장했었던 스위치 리스트저장
        public List<string> swNameList; //DatabaseManager에 저장했었던 스위치이름 저장
        public List<string> varNameList; //DatabaseManager에 저장했었던 varName 저장
        public List<float> varNumberList; //DatabaseManager에 저장했었던 varNumber 저장
    }

    private PlayerManager thePlayer;
    private PlayerStat thePlayerStat;
    private DatabaseManager theDatabase;
    private Inventory theInven;
    private Equipment theEquip;
    private FadeManager theFade;

    private Bound theBound; //내가만든거

    public Data data;

    private Vector3 vector; //여기에 저장된 플레이어위치를 다시 load 해줌\


    public void CallSave() //저장
    {
        #region 데이터 저장과정
        theDatabase = FindObjectOfType<DatabaseManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theEquip = FindObjectOfType<Equipment>();
        theInven = FindObjectOfType<Inventory>();
        theBound = FindObjectOfType<Bound>();

        data.playerX = thePlayer.transform.position.x;
        data.playerY = thePlayer.transform.position.y;
        data.playerZ = thePlayer.transform.position.z;

        data.playerLv = thePlayerStat.character_Lv;
        data.playerHP = thePlayerStat.hp;
        data.playerMP = thePlayerStat.mp;

        data.playerCurrentHP = thePlayerStat.currentHP;
        data.playerCurrentMP = thePlayerStat.currentMP;
        data.playerCurrentEXP = thePlayerStat.currentEXP;

        data.playerATK = thePlayerStat.atk;
        data.playerDEF = thePlayerStat.def;
        data.playerMPR = thePlayerStat.recover_hp;
        data.playerHPR = thePlayerStat.recover_mp;

        data.added_atk = theEquip.added_atk;
        data.added_def = theEquip.added_def;
        data.added_hpr = theEquip.added_hpr;
        data.added_mpr = theEquip.added_mpr;

        data.mapName = thePlayer.currentMapName;
        data.sceneName = thePlayer.currentSceneName;


        Debug.Log("기초 데이터 저장 성공");

        data.playerItemInventory.Clear(); //clear해주지않으면 저장하고 로드할때 아이템이 복사가됨
        data.playerItemInventoryCount.Clear();
        data.playerEquipItem.Clear();

        for(int i = 0; i < theDatabase.var_name.Length; i++) 
        {//DatabaseManager 에 있는 변수 저장
            data.varNameList.Add(theDatabase.var_name[i]);
            data.varNumberList.Add(theDatabase.var[i]);
        }
        for (int i = 0; i < theDatabase.switch_name.Length; i++)
        {//DatabaseManager 에 있는 스위치 저장
            data.swNameList.Add(theDatabase.switch_name[i]);
            data.swList.Add(theDatabase.switches[i]);
        }

        List<Item> itemList = theInven.SaveItem(); //인벤토리의 보유한 아이템을 불러옴 
        for(int i = 0; i < itemList.Count; i++) //불러온 아이템의 아이템ID와 아이템 개수 저장
        {
            Debug.Log("인벤토리의 아이템 저장완료 : "+itemList[i].itemID);
            data.playerItemInventory.Add(itemList[i].itemID);
            data.playerItemInventoryCount.Add(itemList[i].itemCount);
        }

        for(int i = 0; i < theEquip.equipItemList.Length; i++)
        {
            data.playerEquipItem.Add(theEquip.equipItemList[i].itemID);
            Debug.Log("장착된 아이템 저장 완료 : " + theEquip.equipItemList[i].itemID);
        }
        #endregion

        #region 파일저장 & 변환기능
        BinaryFormatter bf = new BinaryFormatter(); //변환하는 기능
        FileStream file = File.Create(Application.dataPath + "/SaveFile.dat");//입출력 기능

        bf.Serialize(file, data);
        file.Close();
        Debug.Log(Application.dataPath + "의 위치에 저장했습니다.");
        #endregion

        
    }

    public void CallLoad() //불러오기 -> 저장기능의 반대로 실행해야함
    {
        #region 파일열기 & 변환기능
        BinaryFormatter bf = new BinaryFormatter(); //변환하는 기능
        FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open);//파일 여는 기능
        #endregion

        #region 파일 불러오기 과정
        if (file != null && file.Length > 0) //파일이 존재하고, 그 안에 내용이 있을경우 Load 실행
        {
            data = (Data)bf.Deserialize(file); //직렬화 한 파일을 Data 형태로 명시적 변환한 후 저장

            theDatabase = FindObjectOfType<DatabaseManager>();
            thePlayer = FindObjectOfType<PlayerManager>();
            thePlayerStat = FindObjectOfType<PlayerStat>();
            theEquip = FindObjectOfType<Equipment>();
            theInven = FindObjectOfType<Inventory>();
            theFade = FindObjectOfType<FadeManager>();

            theFade.FadeOut();

            //data에 저장한 것을 불러왔기 때문에 저장되어있던 내용들을 다시 불러올 수 있음
            thePlayer.currentMapName = data.mapName;
            thePlayer.currentSceneName = data.sceneName;

            vector.Set(data.playerX, data.playerY, data.playerZ);
            thePlayer.transform.position = vector;

            thePlayerStat.character_Lv = data.playerLv;
            thePlayerStat.hp = data.playerHP;
            thePlayerStat.mp = data.playerMP;
            thePlayerStat.currentHP = data.playerCurrentHP;
            thePlayerStat.currentMP = data.playerCurrentMP;
            thePlayerStat.currentEXP = data.playerCurrentEXP;
            thePlayerStat.atk = data.playerATK;
            thePlayerStat.def = data.playerDEF;
            thePlayerStat.recover_hp = data.playerHPR;
            thePlayerStat.recover_mp = data.playerMPR;

            theEquip.added_atk = data.added_atk;
            theEquip.added_def = data.added_def;
            theEquip.added_hpr = data.added_hpr;
            theEquip.added_mpr = data.added_mpr;

            theDatabase.var = data.varNumberList.ToArray(); //ToArray() 함수를 사용해서 리스트에 순서대로 넣어줌
            theDatabase.var_name = data.varNameList.ToArray();
            theDatabase.switches = data.swList.ToArray();
            theDatabase.switch_name = data.swNameList.ToArray();

            //save 이전에 장착하고있던 장비 다시착용
            for (int i = 0; i < theEquip.equipItemList.Length; i++)
            {//플레이어가 장착한 아이템이 있었는지 0~11까지 찾아봄
                for(int x = 0; x < theDatabase.itemList.Count; x++)
                {//Database에 저장된 아이템리스트를 찾아서
                    if (data.playerEquipItem[i] == theDatabase.itemList[x].itemID)
                    {//플레이어가 장착했던 아이템과 같은 ID를 가진 아이템은 다시 장착
                        theEquip.equipItemList[i] = theDatabase.itemList[x];
                        Debug.Log("장착된 아이템을 로드했습니다 : " + theEquip.equipItemList[i].itemID);
                        break;
                    }
                }
            }
          
            List<Item> itemList = new List<Item>();
            //save이전에 인벤토리에 있던 아이템 다시 인벤토리에 넣어줌
            for (int i = 0; i < data.playerItemInventory.Count; i++)
            {//인벤토리에 저장됐던 개수만큼 찾아봄
                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {//Database에 저장된 아이템리스트를 찾아서
                    if (data.playerItemInventory[i] == theDatabase.itemList[x].itemID)
                    {//플레이어가 소지했던 아이템과 같은 ID를 가진 아이템은 다시 인벤토리에 추가
                        itemList.Add(theDatabase.itemList[x]);
                        Debug.Log("인벤토리 아이템을 로드했습니다 : " + theDatabase.itemList[x].itemID);
                        break;
                    }
                }
            }
            
            //소지하고 있던 아이템을 인벤토리에 넣었으면 그 아이템의 개수를 이전과 같이 맞춰줌
            for(int i = 0; i < data.playerItemInventoryCount.Count; i++)
            {
                itemList[i].itemCount = data.playerItemInventoryCount[i];
            }


            theInven.LoatItem(itemList); //인벤토리아이템리스트가 itemList로 교체되면서 저장되어있던 아이템들이 load됨
            theEquip.ShowTxT(); //이 함수를 호출하지않으면 장비창을 띄웠을때 추가되는 스텟이 없음

            StartCoroutine(WaitCoroutine());
           
        }
        else
        {
            Debug.Log("저장된 세이브 파일이 없습니다.");
        }
        #endregion

        file.Close();
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(2f);

        GameManager theGM = FindObjectOfType<GameManager>();
        theGM.LoadStart();

        //tip!
        //->씬이동이 이루어지면 이동이 일어난 이후에 카메라 설정을 해야함
        //-> 카메라 바운드로 설정해야할 BoxCollider가 다른 씬에 있다면, 불러올수가없기때문
        //(-> 현재씬과 다른 씬에있는 객체들은 참조가 불가능하기때문)
        //그래서 씬 이동이 이루어지고 그 씬에 붙어있는 맵의 바운드를 참조해야함
        SceneManager.LoadScene(data.sceneName);
        //하지만 씬이동이 이루어진후에 바운드를 설정하기에는 씬이동이 이루어지면
        //그 이후의 명령어들이 실행이되지 않아서 씬이동후 바운드설정이 불가능

        //=> 그래서 다른 스크립트를 이용해 씬이동 이후의 명령어들을 작성해야함

    }
}