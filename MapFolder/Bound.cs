using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//다른씬으로의 맵이동할때 바운드 바꾸는 방법
public class Bound : MonoBehaviour
{
    private BoxCollider2D bound;
    public string boundName; //save load기능중 카메라의 bound 설정을 하기 위해 구현해줘야함

    private CameraManager theCamera; // CameraManager의 setBound 함수를 이용하기위해 선언

    // Start is called before the first frame update
    void Start()
    {
        bound = GetComponent<BoxCollider2D>(); //bound 스크립트에서 boxcollider2D를 컨트롤할 수 있음
        theCamera = FindObjectOfType<CameraManager>();
    }

    public void SetBound() //load 할떄 사용
    {
        if (theCamera != null)
        {
            theCamera.SetBound(bound);
        }
    }
}