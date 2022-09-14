using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private PlayerManager thePlayer; //이벤트 도중에 키입력 처리방지
    private List<MovingObject> characters;
    //배열의크기는 한번 지정해두면 후에 바꿀 수 없음
    //MovingObject[10] -> 100 / 50
    //NPC A마을 - 10마리
    //NPC B마을 - 20마리 : 초과해서 오류 생김 => List를 사용함으로써 해결
    //list에는 Add(), Remove(), Clear() 메소드가 존재하는데 이것들을 활용해서 리스트의 개수를 늘리고 줄이고 자유롭게 사용가능
    
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    public void PreLoadCharacter()
    {
        characters = ToList();
    }

    //이 함수가 호출이 안되면 characterList는 null 값을 가지게 된다
    //null 값을 가지면 for문을 돌려봤자 어차피 아무 값도 나오지 않음 -> 반드시 호출해야함
    public List<MovingObject> ToList()
    {
        List<MovingObject> tempList = new List<MovingObject>();
        MovingObject[] temp = FindObjectsOfType<MovingObject>(); //MovingObject가 달린 모든 객체를 찾아서 반환시켜줌

        for(int i = 0; i < temp.Length; i++)
        {
            tempList.Add(temp[i]);
        }

        return tempList;
    }

    public void NotMove()
    {
        thePlayer.notMove = true;
    }

    public void Move()
    {
        thePlayer.notMove = false;
    }

    public void SetThorought(string _name) //벽뚫기
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName) //캐릭터 리스트에 있는이름과 파라미터로 넘긴 npc이름이 같은 경우만 Move함수 실행
            {
                characters[i].boxCollider.enabled = false;
            }
        }
    }
    public void SetUnThorought(string _name) //벽생성
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName) //캐릭터 리스트에 있는이름과 파라미터로 넘긴 npc이름이 같은 경우만 Move함수 실행
            {
                characters[i].boxCollider.enabled = true;
            }
        }
    }

    public void SetTransparent(string _name) //투명도
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName) //캐릭터 리스트에 있는이름과 파라미터로 넘긴 npc이름이 같은 경우만 Move함수 실행
            {
                characters[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetUnTransparent(string _name) //투명도
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName) //캐릭터 리스트에 있는이름과 파라미터로 넘긴 npc이름이 같은 경우만 Move함수 실행
            {
                characters[i].gameObject.SetActive(true);
            }
        }
    }

    public void Move(string _name,string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName) //캐릭터 리스트에 있는이름과 파라미터로 넘긴 npc이름이 같은 경우만 Move함수 실행
            {
                characters[i].Move(_dir); //frequency를 생략해서 5로 자동으로 채워짐
            }
        }
    }

    public void Turn(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName) //캐릭터 리스트에 있는이름과 파라미터로 넘긴 npc이름이 같은 경우만 Move함수 실행
            {
                characters[i].animator.SetFloat("DirX", 0f);
                characters[i].animator.SetFloat("DirY", 0f);
                switch (_dir)
                {
                    case "UP":
                        characters[i].animator.SetFloat("DirY", 1f);
                        break;
                    case "DOWN":
                        characters[i].animator.SetFloat("DirY", -1f);
                        break;
                    case "LEFT":
                        characters[i].animator.SetFloat("DirX", -1f);
                        break;
                    case "RIGHT":
                        characters[i].animator.SetFloat("DirX", 1f);
                        break;
                }
            }
        }
    }
}
