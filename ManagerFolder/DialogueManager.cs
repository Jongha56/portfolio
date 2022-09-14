using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//캐릭터 대화창 띄우는 스크립트

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance; //싱글턴화

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

    public Text text;
    public SpriteRenderer rendererSprite; //sprite - audioclip , spriterenderer - audiosource
    public SpriteRenderer rendererDialogueWindow;

    //커스텀클래스에서 만든 배열을 여기다가 넣기위해 선언
    private List<string> listSentences;
    private List<Sprite> listSprites;
    private List<Sprite> listDialogueWindows;

    private int count; //대화 진행 상황 카운트

    public Animator animSprite;
    public Animator animDialogueWindow;

    public string typeSound;
    public string enterSound;

    private AudioManager theAudio;

    public bool talking = false; //대화가 끝나면 종료하기위한 flag
    private bool keyActivated = false; //z키를 연타할경우 캐릭터창이 안뜨는 것을 방지
    private bool onlyText = false; //텍스트창만 띄울지 Dialogue창 전부 띄울지 선택하는 flag


    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        text.text = "";
        listSentences = new List<string>();
        listSprites = new List<Sprite>();
        listDialogueWindows = new List<Sprite>();
        theAudio = FindObjectOfType<AudioManager>();

    }

    public void ShowText(string[] _sentences)
    {
        talking = true;
        onlyText = true;

        for (int i = 0; i < _sentences.Length; i++)
        {
            listSentences.Add(_sentences[i]);
        }

        StartCoroutine(StartTextCoroutine());
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        talking = true;
        onlyText = false;

        for(int i = 0; i < dialogue.sentences.Length; i++)
        {
            listSentences.Add(dialogue.sentences[i]);
            listSprites.Add(dialogue.sprites[i]);
            listDialogueWindows.Add(dialogue.dialogueWindows[i]);
        }

        animSprite.SetBool("Appear", true); //대화창 캐릭터 모습 띄우기
        animDialogueWindow.SetBool("Appear", true); //대화창 모습 띄우기
        StartCoroutine(StartDialogueCoroutine());

    }

    public void ExitDialogue() //초기화과정
    {
        text.text = "";
        count = 0;
        listSentences.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();
        animSprite.SetBool("Appear", false); //대화창 캐릭터 모습 지우기
        animDialogueWindow.SetBool("Appear", false); //대화창 모습 지우기
        talking = false;
    }

    IEnumerator StartTextCoroutine() //간단한 텍스트만 출력하고 싶을 때
    {
        keyActivated = true;
        //대사를 위한 for문
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i];//i번째 문장의 첫글자부터 한글자씩 출력
            if (i % 7 == 1)
            {
                theAudio.Play(typeSound);
            }
            yield return new WaitForSeconds(0.01f);
        }

    }

    IEnumerator StartDialogueCoroutine()
    {
        if (count > 0) //count가 0일때 조건문에서 list[-1]이 없기 때문에 에러가남
        {
            //대사바 교체 확인
            if (listDialogueWindows[count] != listDialogueWindows[count - 1]) //전의 이미지와 비교해서 다르면 교체
            {//대사바가 달라질경우 
                //대사바와 캐릭터 둘 다 교체
                animSprite.SetBool("Change", true);
                animDialogueWindow.SetBool("Appear", false);
                yield return new WaitForSeconds(0.2f);
                rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count]; //listDialogue에 있는 이미지를 rendererDialogue에 넣어줌
                rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count]; //listSprites에 있는 이미지를 rendererSprite에 넣어줌
                animDialogueWindow.SetBool("Appear", true);
                animSprite.SetBool("Change", false);
            }
            else //대사바가 같을경우
            {
                //이미지가 바뀔경우 sprite만 교체
                //sprite 교체 확인
                if (listSprites[count] != listSprites[count - 1]) //전의 이미지와 비교해서 다르면 교체
                {
                    animSprite.SetBool("Change", true);
                    yield return new WaitForSeconds(0.1f);
                    rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count]; //listSprites에 있는 이미지를 rendererSprite에 넣어줌
                    animSprite.SetBool("Change", false);
                }
                else //대사바도 같고 이미지도 같을경우 대기
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }
            
        }
        else //첫문장일때 이미지 교체 한번 실행
        {
            rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count]; //listDialogue에 있는 이미지를 rendererDialogue에 넣어줌
            rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count]; //listSprites에 있는 이미지를 rendererSprite에 넣어줌
        }

        keyActivated = true;
        //대사를 위한 for문
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i];//i번째 문장의 첫글자부터 한글자씩 출력
            if (i % 7 == 1)
            {
                theAudio.Play(typeSound);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (talking && keyActivated)
        {
            if (Input.GetKeyDown(KeyCode.Space)) //z키가 눌렷을때 대화창을 다음문장으로 넘김
            {
                keyActivated = false;
                count++;
                text.text = ""; //대화읽는 중 z키 눌렀을 경우 대화창 초기화하고 다시 띄워줌
                theAudio.Play(enterSound);

                if (count == listSentences.Count) //count의 수가 문장의 수와 같을경우 코루틴 종료
                {
                    StopAllCoroutines();
                    ExitDialogue();
                }
                else //대화를 읽는 중 z키 눌렀을경우
                {
                    StopAllCoroutines();
                    if (onlyText)
                        StartCoroutine(StartTextCoroutine());
                    else
                        StartCoroutine(StartDialogueCoroutine());
                }
            }
        }
    }
}
