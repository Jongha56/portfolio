using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//save 후 load과정에서 카메라의 바운드설정 등 SaveNLoad에서 LoadScene이후에 카메라 바운드 설정과정
public class GameManager : MonoBehaviour
{
    private Bound[] bounds;
    private PlayerManager thePlayer;
    private CameraManager theCamera;
    private FadeManager theFade;

    //MenuCanvas 와 SystemCanvas 가 씬이동이 이루어지면서 메인카메라를 놓쳐서 이상하게 나오는것을 방지하기위해선언
    private Menu theMenu;
    private DialogueManager theDM;
    private Camera cam;

    public GameObject hpbar;
    public GameObject mpbar;

    public void LoadStart()
    {
        StartCoroutine(LoadWaitCoroutine());
    }
    IEnumerator LoadWaitCoroutine()
    {
        //대기시간없이 바로 실행시켜버리면 다른 스크립트의 함수(FindObjectOfType 등)이
        //실행되기도 전에 먼저 코루틴이 일어나서 오류가 생김
        yield return new WaitForSeconds(0.5f); //대기시간을 둬서 다른함수들이 find 참조될때까지 기다려줘야함

        thePlayer = FindObjectOfType<PlayerManager>();
        bounds = FindObjectsOfType<Bound>(); //LoadStart를 할떄마다 해당 씬에있는 바운드로 바뀜
        theCamera = FindObjectOfType<CameraManager>();
        theFade = FindObjectOfType<FadeManager>();
        theMenu = FindObjectOfType<Menu>();
        theDM = FindObjectOfType<DialogueManager>();
        cam = FindObjectOfType<Camera>();

        //title에 있는 캐릭터는 투명한 상태이기때문에 다시 보이게 만들어줘야함
        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;

        //하이어라키에있는 Player 라는 객체의 이름을 찾아서 camera의 target으로 넣어줌
        theCamera.target = GameObject.Find("Player");

        //Menu 와 DialogueManager에서 Canvas에 붙어있는 카메라를 찾아 메인카메라를 넣어줌
        theMenu.GetComponent<Canvas>().worldCamera = cam;
        theDM.GetComponent<Canvas>().worldCamera = cam;


        for (int i = 0; i < bounds.Length; i++)
        {
            if (bounds[i].boundName == thePlayer.currentMapName)
            {//player가 있는 맵의 이름과 bound 배열에 저장했던 이름과 같으면
                bounds[i].SetBound(); //카메라의 바운드를 해당 바운드로 조정
                break;
            }
        }

        hpbar.SetActive(true);
        mpbar.SetActive(true);

        theFade.FadeIn(); //씬이동전 fade out을 하고 씬이동후 여기에서 fade in을 해줌
    }
}
