using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //커스텀클래스
public class Dialogue 
{
    [TextArea(1,2)]
    public string[] sentences; //캐릭터의 대화창에 나올 문장들
    public Sprite[] sprites; //누가 말하는지 모습 바꿔주기위해 선언하는 배열
    public Sprite[] dialogueWindows; //대화창 바꿔주기위해 선언하는 배열
}
