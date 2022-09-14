using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntEnemy : MonoBehaviour
{
    public GameObject prefabs_Floating_Text;
    public GameObject parent; //canvas
    public GameObject effect; //몬스터 때릴때 이펙트

    public string atkSound;

    private PlayerStat thePlayerStat;


    // Start is called before the first frame update
    void Start()
    {
        thePlayerStat = FindObjectOfType<PlayerStat>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            int dmg = collision.gameObject.GetComponent<EnemyStat>().Hit(thePlayerStat.atk);
            AudioManager.instance.Play(atkSound);

            Vector3 vector = collision.transform.position; //데미지 받은 텍스트 띄워주기위한 좌표값

            Instantiate(effect, vector, Quaternion.Euler(Vector3.zero)); //때릴때 이펙트가 몸에서 터지고 y값증가시켜서 데미지는 머리위에뜨게함

            vector.y += 60; //캐릭터의 y값보다 60위에

            GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<FloatingText>().text.text = dmg.ToString();
            clone.GetComponent<FloatingText>().text.color = Color.white;
            clone.GetComponent<FloatingText>().text.fontSize = 25;
            clone.transform.SetParent(parent.transform);
        }
    }
}
