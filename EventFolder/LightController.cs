using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private PlayerManager thePlayer; //플레이어가 바라보고 있는 방향
    private Vector2 vector; //바라보고있는 방향을 저장시킬 변수

    private Quaternion rotation; //회전(각도)을 담당하는 Vector4(x,y,z,w)

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = thePlayer.transform.position;

        vector.Set(thePlayer.animator.GetFloat("DirX"), thePlayer.animator.GetFloat("DirY"));

        if (vector.x == 1f) //오른쪽보고있을경우
        {
            rotation = Quaternion.Euler(0, 0, 90);
            this.transform.rotation = rotation; //Quaternion을 사용하기 때문에 Vector4를 사용할 수 없음
        }
        else if (vector.x == -1f) //왼쪽
        {
            rotation = Quaternion.Euler(0, 0, -90);
            this.transform.rotation = rotation;
        }
        else if (vector.y == 1f) //위
        {
            rotation = Quaternion.Euler(0, 0, 180);
            this.transform.rotation = rotation;
        }
        else if (vector.y == -1f) //아래
        {
            rotation = Quaternion.Euler(0, 0, 0);
            this.transform.rotation = rotation;
        }
    }
}
