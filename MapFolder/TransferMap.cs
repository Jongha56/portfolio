using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //LoadScene 명령어를 사용하기 위한 라이브러리

//다른씬의 맵으로 이동하는 방법과
//같은씬의 맵으로 이동하는 방법을
//하나의 스크립트에 섞어쓰는 건 추천하지 않음

//같은씬에서의 맵이동할때 바운드바꾸는 방법
public class TransferMap : MonoBehaviour
{
    public string transferMapName; //이동할 맵의 이름

    public Transform target;
    public BoxCollider2D targetBound; //바운드를 변경할 변수

    public Animator anim_1; //문이 열리는 애니메이션
    public Animator anim_2; //닫히는 애니메이션
    public int door_count;
    [Tooltip("UP, DOWN, LEFT, RIGHT")]
    public string direction; //캐릭터가 바라보고 있는 방향
    private Vector2 vector; //getFloat("DirX")를 저장할 변수
    [Tooltip("문이 있으면 : true, 문이 없으면 : false")]
    public bool door; //문이 있는지 없는지 인스펙터창에서 체크함

    private PlayerManager thePlayer; //player정보 가져올 변수
    private CameraManager theCamera;
    private FadeManager theFade; //맵이동할때 화면조정
    private OrderManager theOrder; //맵이동할때 못움직이게 만들어줘야함
    private WeatherManager theWeather;

    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        //FindObjectOfType 과 GetComponent 의 차이점 => 검색범위의 차이
        //FindObjectOfType<> -> 하이어라키에 있는 모든 객체의 <> 컴포넌트를 검색해서 리턴
        //GetComponent<> -> 해당 스크립트가 적용된 객체의 <> 컴포넌트를 검색해서 리턴
        //FindObjectOfType<> : 다수의 객체 참조 / GetComponent<> : 단일 객체 참조
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theWeather = FindObjectOfType<WeatherManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    { //문이 없을경우
        if (!door)
        {
            //박스컬라이더에 닿은 오브젝트의 이름이 player이면 실행
            if (collision.gameObject.name == "Player")
            {
                if (Input.GetKeyDown(KeyCode.Z))
                    StartCoroutine(TransferCoroutine());
            }
        }
        //문이 있는 경우
        if (door)
        {
            if (collision.gameObject.name == "Player")
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    theWeather.RainStop();
                    vector.Set(thePlayer.animator.GetFloat("DirX"), thePlayer.animator.GetFloat("DirY"));
                    switch (direction)
                    {
                        case "UP":
                            if(vector.y==1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "DOWN":
                            if (vector.y == -1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "RIGHT":
                            if (vector.x == 1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "LEFT":
                            if (vector.x == -1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        default:
                            StartCoroutine(TransferCoroutine());
                            break;
                    }
                }
                
            }
        }
    }

    IEnumerator TransferCoroutine()
    {
        theOrder.PreLoadCharacter(); //SetTransparent를 사용하려면 player를 찾아야하는데 player를 찾으려면 list에 player가 있어야함
        theOrder.NotMove();
        theFade.FadeOut();
        if (door) //문이있으면 열리는 에니메이션 실행
        {
            anim_1.SetBool("Open", true);
            if(door_count==2)
                anim_2.SetBool("Open", true);
        }
        yield return new WaitForSeconds(0.3f);

        theOrder.SetTransparent("player"); //캐릭터를 투명하게 만들어줌
        if (door) //문이있으면 닫히는 에니메이션도 실행
        {
            anim_1.SetBool("Open", false);
            if (door_count == 2)
                anim_2.SetBool("Open", false);
        }


        yield return new WaitForSeconds(0.7f);
        theOrder.SetUnTransparent("player");

        thePlayer.currentMapName = transferMapName;
        theCamera.SetBound(targetBound); //맵의 바운드를 변화시키는 함수

        theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
        thePlayer.transform.position = target.transform.position;
        theFade.FadeIn();
        yield return new WaitForSeconds(0.5f); //맵이 밝아지기전에 움직이는것방지

        if (door)
        {
            anim_1.SetBool("End", true);
            if (door_count == 2)
                anim_2.SetBool("End", true);
        }

        theOrder.Move();
    }
}