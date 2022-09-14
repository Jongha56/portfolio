using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStat : MonoBehaviour
{
    public int hp;
    public int currentHp;
    public int atk;
    public int def;
    public int exp;

    //몬스터가 플레이어한테 맞을때만 몬스터의 hp상태 나타내줌
    public GameObject healthBarBackground;
    public Image healthBarFilled;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        healthBarFilled.fillAmount = 1f; //기본값으로 몬스터 풀피 fillAmount는 0~1사이값이어야함

    }

    public int Hit(int _playerAtk)
    {
        int playerAtk = _playerAtk;
        int dmg;
        if (def >= playerAtk)
            dmg = 0;
        else
            dmg = playerAtk - def;

        currentHp -= dmg;

        if (currentHp <= 0)
        {
            Destroy(this.gameObject);
            PlayerStat.instance.currentEXP += exp;
        }

        healthBarFilled.fillAmount = (float)currentHp / hp;
        healthBarBackground.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(WaitCoroutine());
        return dmg;

    }

    IEnumerator WaitCoroutine()
    { //플레이어가 몬스터를 떄리고 도망간 후 3초가 지나면 몬스터의 체력바를 안보이게 해줌
        yield return new WaitForSeconds(3f);
        healthBarBackground.SetActive(false);
    }
}
