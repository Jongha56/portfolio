using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//다른 씬으로 맵 이동할 때
public class TransferScene : MonoBehaviour
{
    public string transferSceneName; //이동할 씬의 이름


    public Animator anim_1; //문이 열리는 애니메이션
    public Animator anim_2; //닫히는 애니메이션
    public int door_count;
    [Tooltip("UP, DOWN, LEFT, RIGHT")]
    public string direction; //캐릭터가 바라보고 있는 방향
    private Vector2 vector; //getFloat("DirX")를 저장할 변수
    [Tooltip("문이 있으면 : true, 문이 없으면 : false")]
    public bool door; //문이 있는지 없는지 인스펙터창에서 체크함

    private PlayerManager thePlayer;
    private FadeManager theFade; //맵이동할때 화면조정
    private OrderManager theOrder; //맵이동할때 못움직이게 만들어줘야함

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
    }

    //트리거를 활성화 시킨 박스컬라이더에 닿는 순간 실행되는 내장합수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            thePlayer.currentMapName = transferSceneName;
            SceneManager.LoadScene(transferSceneName);
        }
    }
}
