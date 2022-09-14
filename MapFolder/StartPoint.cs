using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string startPoint; // 맵이 이동, 플레이어가 시작될 위치
    //transferMapName 과 startPoint 가 같은 곳으로 이동

    private PlayerManager thePlayer; //MovingObject에 있는 Player 참조 -> transferMapName 가져옴
    private CameraManager theCamera;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        if (startPoint == thePlayer.currentMapName)
        {
            theCamera.transform.position=new Vector3(this.transform.position.x,this.transform.position.y,theCamera.transform.position.z);
            thePlayer.transform.position = this.transform.position;
        }
    }
}
