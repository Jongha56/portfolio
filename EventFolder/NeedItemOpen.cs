using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//아이템을 보유하고 있는 경우에만 맵이동 가능
public class NeedItemOpen : MonoBehaviour
{
    public Dialogue dialogue_1;

    private DialogueManager theDM;

    public string transferMapName; //이동할 맵의 이름

    public Transform target;
    public BoxCollider2D targetBound; //바운드를 변경할 변수

    public int Need_ItemID; // 필요한 아이템 ID
    private bool Exist_Item = false; //아이템이 존재하면 true 없으면 false
    private bool flag = false;
    
    private PlayerManager thePlayer; //player정보 가져올 변수
    private CameraManager theCamera;
    private FadeManager theFade; //맵이동할때 화면조정
    private OrderManager theOrder; //맵이동할때 못움직이게 만들어줘야함
    private Inventory theInven;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theInven = FindObjectOfType<Inventory>();
        theDM = FindObjectOfType<DialogueManager>();

    }

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



    IEnumerator TransferCoroutine()
    {
        theOrder.PreLoadCharacter(); //SetTransparent를 사용하려면 player를 찾아야하는데 player를 찾으려면 list에 player가 있어야함
        theOrder.NotMove();
        theFade.FadeOut();

        yield return new WaitForSeconds(0.7f);
        theOrder.SetUnTransparent("player");

        thePlayer.currentMapName = transferMapName;
        theCamera.SetBound(targetBound); //맵의 바운드를 변화시키는 함수

        theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
        thePlayer.transform.position = target.transform.position;
        theFade.FadeIn();
        yield return new WaitForSeconds(0.5f); //맵이 밝아지기전에 움직이는것방지
        
        theOrder.Move();
    }

    IEnumerator NotTransferCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking); //대화가 끝날때까지 대기하다가 끝나면 밑문장 실행

        theOrder.Move();

    }
}
