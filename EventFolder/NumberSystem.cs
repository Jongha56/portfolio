using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//이벤트 발생시 정답을 맞출경우 상자가 열리는 등의 모션을 줄 수 있음
public class NumberSystem : MonoBehaviour
{
    private AudioManager theAudio;
    public string key_sound; //방향키 사운드
    public string enter_sound; //결정키 사운드
    public string cancel_sound; //오답 & 취소키 사운드
    public string correct_sound; //정답 사운드

    private int count; //배열의 크기, 비밀번호의 자릿수 1000->count=3
    private int selectedTextbox; //선택된 자릿수
    private int result; //플레이어가 도출해낸 값
    private int correctNumber; //정답

    private string tempNumber;

    public int sorting_center; //panel이 자릿수에 맞춰 가운데 정렬할 수 있게 위치값조정하는 변수 "40"이 적당함
    public GameObject superObject; //슈퍼오브젝트를 자릿수에 맞게 화면 가운데 정렬
    public GameObject[] panel; //필요한 자릿수의 갯수만큼 활성화. 비활성화
    public Text[] Number_Text; //text 숫자

    public Animator anim;

    public bool activated; //패스워드 작업이 끝날때까지 대기를 위한 역할 / = return new waitUntil
    private bool keyInput; //키처리 활성화, 비활성화
    private bool correctFlag; //정답인지 아닌지 여부확인 true-> 정답. false->오답

    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void ShowNumber(int _correctNum)
    {
        correctNumber = _correctNum;
        activated = true; //대기
        correctFlag = false; //답도 적지 않았는데 정답일 수는 없기 때문에 false로 초기화

        string temp = correctNumber.ToString(); //"143451" 문자열로 만들어서 -> length 속성 이용
        for (int i = 0; i < temp.Length; i++)
        {
            count = i; //숫자의 자릿수를 count에 넣음
            panel[i].SetActive(true); //panel을 자릿수만큼 활성화시켜줌
            Number_Text[i].text = "0";
        }
        superObject.transform.position = new Vector3(superObject.transform.position.x + (sorting_center * count), superObject.transform.position.y, superObject.transform.position.z);
        
        selectedTextbox = 0; //첫번째 자리부터 선택하기위해 초기화
        result = 0; //플레이어가 아직 답을 도출하지 않았기 떄문에 0으로 초기화
        SetColor(); //첫번째 자리수가 선택된 것을 표시
        anim.SetBool("Appear", true);
        keyInput = true;

        
    }

    public bool GetResult() //정답과 오답을 알려줄 함수
    {
        return correctFlag;
    }

    public void SetNumber(string _arrow) //panel의 번호를 바꿔주는 함수
    {
        int temp = int.Parse(Number_Text[selectedTextbox].text); //선택된 panel의 텍스트를 integer 형으로 강제 형변환

        if (_arrow == "DOWN")
        {
            if (temp == 0)
                temp = 9;
            else
                temp--;
        }
        else if(_arrow == "UP")
        {
            if (temp == 9)
                temp = 0;
            else
                temp++;
        }
        Number_Text[selectedTextbox].text = temp.ToString(); //다시 string형으로 형변환
    }

    public void SetColor() //선택된 panel을 선택되지 않은 panel과 다르게 보이도록 바꿔줄 함수
    {
        Color color = Number_Text[0].color; //첫번쨰자리는 무조건 있기 때문에 Number_Text[0]의 color를 불러옴
        color.a = 0.3f;
        for(int i = 0; i <= count; i++) //모든 패널의 투명도를 0.3f로 만들어줌
        {
            Number_Text[i].color = color;
        }
        color.a = 1f;
        Number_Text[selectedTextbox].color = color; //선택된 panel의 투명도를 1f로
    }

    // Update is called once per frame
    void Update()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow)) //아래키 누를경우 -> 숫자 내려감
            {
                theAudio.Play(key_sound);
                SetNumber("DOWN");
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow)) //위키 누를경우 -> 숫자 올라감
            {
                theAudio.Play(key_sound);
                SetNumber("UP");
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) //왼쪽키 누를경우
            {
                theAudio.Play(key_sound);
                if (selectedTextbox < count) //최대자리수보다 작을떄 selectedTextbox 1씩 증가
                    selectedTextbox++;
                else //최대자리수만큼 이동했을때 selectedTextbox 원래자리로 되돌아옴
                    selectedTextbox = 0;
                SetColor();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) //오른쪽키 누를경우 
            {
                theAudio.Play(key_sound);
                if (selectedTextbox > 0) //0보다 크면 selectedTextbox 1씩 감소
                    selectedTextbox--;
                else //0까지 이동했을 때 selectedTextbox를 최대자리수로 초기화
                    selectedTextbox = count;
                SetColor();
            }
            else if (Input.GetKeyDown(KeyCode.Z)) //결정키
            {
                theAudio.Play(key_sound);
                keyInput = false;
                StartCoroutine(OXCoroutine());
            }
            else if (Input.GetKeyDown(KeyCode.X)) //취소키
            {
                theAudio.Play(key_sound);
                keyInput = false;
                StartCoroutine(ExitCoroutine());
            }

        }
    }

    IEnumerator OXCoroutine()
    {
        Color color = Number_Text[0].color;
        color.a = 1f; //투명도를 0.3f에서 원래 투명도로 바꿔줌 

        //정답을 비교할때 0부터 for문을 돌리면 숫자가 거꾸로 들어가기때문에 반대로 for문을 돌림
        for (int i = count; i >= 0; i--) 
        {
            Number_Text[i].color = color;
            tempNumber += Number_Text[i].text;
        }

        yield return new WaitForSeconds(1f); //연출 -> 1초동안 모든 숫자가 보임

        result = int.Parse(tempNumber);

        //player의 답과 정답이 같을경우 true, 다를경우 false
        if (result == correctNumber)
        {
            theAudio.Play(correct_sound);
            correctFlag = true;
        }
        else
        {
            theAudio.Play(cancel_sound);
            correctFlag = false;
        }

        StartCoroutine(ExitCoroutine());
    }
    IEnumerator ExitCoroutine()
    {
        //Debug.Log("우리가 낸 답 = " + result + "     정답 = " + correctNumber);
        result = 0;
        tempNumber = "";
        anim.SetBool("Appear", false);
        yield return new WaitForSeconds(0.1f);

        for(int i = 0; i <= count; i++)
        {
            panel[i].SetActive(false);
        }
        superObject.transform.position = new Vector3(superObject.transform.position.x - (sorting_center * count), superObject.transform.position.y, superObject.transform.position.z);

        Debug.Log("x값 : " + superObject.transform.position.x);
        activated = false;
    }
}
