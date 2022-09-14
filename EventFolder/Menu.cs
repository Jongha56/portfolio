using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public static Menu instance;
    private void Awake()
    {
        //캐릭터가 맵을 이동할때마다 자신이 복제가 되기 때문에 조건문을 달아서 
        //처음맵을 이동할때만 파괴시키지 않고 그 뒤부터는 파괴시키게 만듦
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject); //다른 씬으로 갈 때마다 게임오브젝트 파괴방지 
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public GameObject go; //메뉴를 활성화 비활성화 시킬수 있게해주는 go
    public AudioManager theAudio;

    public string call_sound;
    public string cancel_sound;

    public OrderManager theOrder;

    public GameObject[] gos;

    private bool activated;

    public void Exit() //게임종료함수
    {
        Application.Quit();
    }

    public void Continue()
    {
        activated = false;
        go.SetActive(false);
        theOrder.Move();
        theAudio.Play(cancel_sound);
    }
    /*
    public void GoToTitle()
    {
        //아이템들이 중복되는 것을 막기위해 타이틀로 버튼을 누르면 전부 파괴
        for (int i = 0; i < gos.Length; i++)
            Destroy(gos[i]);

        go.SetActive(false);
        activated = false;
        SceneManager.LoadScene("title");
    }
    */
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activated = !activated;

            if (activated)
            {
                theOrder.NotMove();
                go.SetActive(true);
                theAudio.Play(call_sound);
            }
            else
            {
                go.SetActive(false);
                theAudio.Play(cancel_sound);
                theOrder.Move();
            }
        }
    }
}
