using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    public int character_Lv;
    public int[] needExp;
    public int currentEXP;

    public int hp;
    public int currentHP;
    public int mp;
    public int currentMP;

    public int atk;
    public int def;

    public int recover_hp; // 초당 hp 회복
    public int recover_mp;

    public string dmgSound;

    public float time; //초당 회복시키기위해 사용할 시간 변수
    private float current_time;

    public GameObject prefabs_Floating_text;
    public GameObject parent; // canvas

    public Slider hpSlider;
    public Slider mpSlider;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentHP = hp;
        currentMP = mp;
        current_time = time;
    }

    public void Hit(int _enemyAtk)
    {
        int dmg; //최종적으로 받는 데미지

        if (def >= _enemyAtk) //캐릭터의 방어력이 적의 공격력보다 높거나 같으면 데미지 1받음
            dmg = 1;
        else
            dmg = _enemyAtk - def; //적의 공격력 - 캐릭터 방어력

        currentHP -= dmg;

        if (currentHP <= 0)
        {
            hpSlider.value = 0f;
            Destroy(this.gameObject);

            //Debug.Log("체력 0 미만, 게임오버");
        }

        AudioManager.instance.Play(dmgSound);

        Vector3 vector = this.transform.position; //데미지 받은 텍스트 띄워주기위한 좌표값
        vector.y += 60; //캐릭터의 y값보다 60위에

        GameObject clone = Instantiate(prefabs_Floating_text, vector, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingText>().text.text = dmg.ToString();
        clone.GetComponent<FloatingText>().text.color = Color.red;
        clone.GetComponent<FloatingText>().text.fontSize = 25;
        clone.transform.SetParent(parent.transform);
        StopAllCoroutines(); //연속으로 피격될 수 있기 때문에 코루틴종료
        StartCoroutine(HitCoroutine());

    }
    IEnumerator HitCoroutine() //캐릭터가 피격시 깜빡깜빡하는 효과 부여
    {
        Color color = GetComponent<SpriteRenderer>().color; //캐릭터의 sprite를 가져옴
        color.a = 0;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.maxValue = hp;
        mpSlider.maxValue = mp;

        hpSlider.value = currentHP;
        mpSlider.value = currentMP;

        if (currentEXP >= needExp[character_Lv]) //레벨업 했을경우
        {
            character_Lv++;
            hp += character_Lv * 2;
            mp += character_Lv + 2;

            currentHP = hp;
            currentMP = mp;
            atk++;
            def++;
        }

        current_time -= Time.deltaTime;

        if (current_time <= 0)
        {
            if (recover_hp > 0)
            {
                if (currentHP + recover_hp <= hp)
                    currentHP += recover_hp;
                else
                    currentHP = hp;
            }
            current_time = time;
        }
    }
}
