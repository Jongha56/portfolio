using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//아이템을 획득했을 때 어떤 아이템을 획든한 지 알려줌
public class FloatingText : MonoBehaviour
{
    public float moveSpeed;
    public float destroyTime;

    public Text text; //획득한 아이템의 이름 받아올변수

    private Vector3 vector; //위치값 받아올 변수

    // Update is called once per frame
    void Update()
    {
        vector.Set(text.transform.position.x, transform.position.y + (moveSpeed * Time.deltaTime), text.transform.position.z); //1초에 moveSpeed만큼 이동
        text.transform.position = vector;

        destroyTime -= Time.deltaTime;

        if (destroyTime <= 0) //destroyTime 이 0이하가 되면 게임오브젝트 파괴
            Destroy(this.gameObject);
    }
}
