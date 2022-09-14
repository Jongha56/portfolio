using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager instance;

    #region Singleton
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
    #endregion Singleton

    private AudioManager theAudio; //키입력시 사운드 재생

    private string question; //커스텀클래스에 있는 question 대입시키기위한 변수
    private List<string> answerList; //커스텀클래스에 있는 answers배열 대입 시키기 위한 list

    public GameObject go; //평소에 choicemanager를 비활성화 시킬 목적으로 선언 -> setActive로 필요할떄 활성화

    public Text question_Text;
    //선택된 답만 진하게 채우기위해서 선언해줌
    public Text[] answer_Text;
    public GameObject[] answer_Panel;

    public Animator anim;

    public string keySound;
    public string enterSound;

    public bool choiceIng; //()=>!choiceIng , 선택지가 활성화되면 다음 대사창이 나오지 않게 대기 
    private bool keyInput; //키처리 활성화, 비활성화 선택지가 나올떄만 키를 눌렀을때 소리나오게 선택

    private int count; //배열의 크기
    private int result; //선택한 선택창 번호

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
        answerList = new List<string>();
        for (int i = 0; i < answer_Text.Length; i++) //answer 개수 초기화
        {
            answer_Text[i].text = "";
            answer_Panel[i].SetActive(false); //기본적으로 비활성화
        }
        question_Text.text = "";
    }

    public void ShowChoice(Choice _choice)
    {
        go.SetActive(true); //이벤트 발생시 question, answer창이 보이게 설정
        choiceIng = true;
        result = 0;
        question = _choice.question;
        for(int i = 0; i < _choice.answers.Length; i++)
        {
            answerList.Add(_choice.answers[i]);
            answer_Panel[i].SetActive(true); //배열의 크기만큼 패널 활성화
            count = i;
        }
        anim.SetBool("Appear", true);
        Selection(); //다음 문제의 선택지에서 이전문제의 선택지번호가 진해지는 것을 방지하기위해
        // result=0일때 selection을 한번 불러와서 0번째 선택지가 선택되는것으로 보이게 만듦
        StartCoroutine(ChoiceCoroutine());
    }
    IEnumerator ChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.2f); //question,answer 창이 나오는 에니메이션을 20프레임 설정한 것이 실행될동안 대기
        StartCoroutine(TypingQuestion());
        StartCoroutine(TypingAnswer_0());
        if (count >= 1)
            StartCoroutine(TypingAnswer_1());
        if (count >= 2)
            StartCoroutine(TypingAnswer_2());
        if (count >= 3)
            StartCoroutine(TypingAnswer_3());

        yield return new WaitForSeconds(0.5f);
        keyInput = true; 

    }

    public int GetResult() //다른 script에서 result값을 참조하기 위해 사용
    {
        return result;
    }

    public void ExitChoice() //답을 고른 후 모든 것을 초기화 시키는 과정
    {
        question_Text.text = "";
        for (int i = 0; i <= count; i++)
        {
            answer_Text[i].text = "";
            answer_Panel[i].SetActive(false); //다시 비활성화
        }
        answerList.Clear();
        anim.SetBool("Appear", false);
        choiceIng = false;

        go.SetActive(false); //평상시에 qusetion, answer 창이 안보이게 설정
    }

    //동시에 질문과 대답(1~4) 텍스트를 출력하기위해 coroutine을 여러개 만들어줌
    #region Coroutine
    IEnumerator TypingQuestion()
    {
        for(int i = 0; i < question.Length; i++)
        {
            question_Text.text += question[i]; //question 한글자씩 타이필
            yield return waitTime;
        }
    }
    IEnumerator TypingAnswer_0()
    {
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < answerList[0].Length; i++)
        {
            answer_Text[0].text += answerList[0][i]; //question 한글자씩 타이필
            yield return waitTime;
        }
    }
    IEnumerator TypingAnswer_1()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < answerList[1].Length; i++)
        {
            answer_Text[1].text += answerList[1][i]; //question 한글자씩 타이필
            yield return waitTime;
        }
    }
    IEnumerator TypingAnswer_2()
    {
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < answerList[2].Length; i++)
        {
            answer_Text[2].text += answerList[2][i]; //question 한글자씩 타이필
            yield return waitTime;
        }
    }
    IEnumerator TypingAnswer_3()
    {
        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i < answerList[3].Length; i++)
        {
            answer_Text[3].text += answerList[3][i]; //question 한글자씩 타이필
            yield return waitTime;
        }
    }
    #endregion Coroutine

    // Update is called once per frame
    void Update()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) //위화살표
            {
                theAudio.Play(keySound);
                if (result > 0)
                    result--;
                else
                    result = count;
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) //아래화살표
            {
                theAudio.Play(keySound);
                if (result < count)
                    result++;
                else
                    result = 0;
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.Z)) //선택지 고를키
            {
                theAudio.Play(enterSound);
                keyInput = false; //답을 골랐기 때문에 더이상 키입력이 이루어질 필요가 없음
                ExitChoice();
            }
        }
    }
    public void Selection()
    {
        Color color = answer_Panel[0].GetComponent<Image>().color; 
        color.a = 0.75f; 
        for(int i = 0; i <= count; i++) //모든선택창의 투명도를 0.75f 로바꿔줌
        {
            answer_Panel[i].GetComponent<Image>().color = color;
        }
        color.a = 1f; //color의 투명도를 1f 로 바꾼후
        answer_Panel[result].GetComponent<Image>().color = color; //선택된 answer창만 투명도를 1f로 바꿔줌

    }
}
