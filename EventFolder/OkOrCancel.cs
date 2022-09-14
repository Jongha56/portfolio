using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OkOrCancel : MonoBehaviour
{
    private AudioManager theAudio;
    public string key_sound;
    public string enter_sound;
    public string cancel_sound;

    public GameObject up_Panel;
    public GameObject down_Panel;

    public Text up_Text; //사용, 장착 버튼 등등
    public Text down_Text; //취소, 해제 버튼 등등 원하는대로 바꿀수있게 만들어줌

    public bool activated; //고를떄까지 대기할 bool
    private bool keyInput; // true : 키입력 가능 , false : 키입력 불가능
    private bool result = true; //true : 소비(up_Panel) , false : 취소(down_Panel)


    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void Selected()
    {
        theAudio.Play(key_sound);
        result = !result; //기본값 true로 두고 위키 아래키 무엇을 누르던 두가지 경우의 수밖에 없기때문에 bool값이 바뀜

        if (result) 
        {
            up_Panel.gameObject.SetActive(false);
            down_Panel.gameObject.SetActive(true);
        }
        else
        {
            up_Panel.gameObject.SetActive(true);
            down_Panel.gameObject.SetActive(false);
        }
    }

    public void ShowTwoChoice(string _upText, string _downText)
    {
        activated = true;
        result = true;
        up_Text.text = _upText;
        down_Text.text = _downText;

        //기본형태, 위버튼이 활성화된것처럼 보여주기위함
        up_Panel.gameObject.SetActive(false);
        down_Panel.gameObject.SetActive(true);

        StartCoroutine(ShowTwoChoiceCoroutine());
    }

    public bool GetResult()
    {
        return result;
    }

    IEnumerator ShowTwoChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.01f);
        keyInput = true; //z키 중복실행 방지하기 위해서 ShowTwoChoice가 아닌 코루틴에서 true로 만들어줌
    }

    // Update is called once per frame
    void Update()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Selected();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Selected();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                theAudio.Play(enter_sound);
                keyInput = false;
                activated = false;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                theAudio.Play(cancel_sound);
                keyInput = false;
                activated = false;
                result = false;

            }
        }   
    }
}
