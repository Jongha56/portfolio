using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static public CameraManager instance;

    public GameObject target; //카메라가 따라갈 대상
    public float moveSpeed; //카메라가 얼마나 빠른 속도로 움직일지
    private Vector3 targetPosition; //대상의 현재 위치 값

    public BoxCollider2D bound; //카메라가 맵 밖으로 이동되는 것 방지

    // 박스 컬라이더 영역의 최소, 최대 xyz값을 지님
    private Vector3 minBound;
    private Vector3 maxBound;

    //카메라의 반너비, 반높이 값을 지닐 변수
    private float halfWidth;
    private float halfHeight;

    //카메라의 반높이값을 구할 속성을 이용하기 위한 변수
    private Camera theCamera;

    //start보다 먼저 실행되는 함수
    private void Awake()
    {
        //처음 실행될 때는 카메라를 파괴하지 않고
        //다음 실행부터 카메라가 중복생성되는 것을 방지하기 위해서 파괴해줌
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject); //다른 씬으로 갈 때마다 게임오브젝트 파괴방지

        theCamera = GetComponent<Camera>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = theCamera.orthographicSize; //카메라의 반높이
        halfWidth = halfHeight * Screen.width / Screen.height; //카메라 반너비구하는 공식

    }

    // Update is called once per frame
    void Update()
    {
        //매 프레임마다 카메라가 대상을 쫓아야 하기 때문에 update에 설정
        if (target.gameObject != null)
        {
            //this를 사용하는 이유 : 대상의 위치가 아니라 카메라가 대상을 바라봐야해서 자기 자신의 값을 계속 가지고 있어야 하기 때문에
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

            //1초에 moveSpeed만큼 이동
            //Lerp는 A값과 B값 사이의 선형 보간으로 중간 값을 리턴
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            float clampedX = Mathf.Clamp(this.transform.position.x,minBound.x+halfWidth,maxBound.x-halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);
            // (10:value, 0:min, 100:max) -> return 10
            // (-100, 0, 100) -> return 0
            // (1000, 0, 100) -> return 100  => 범위안에 리턴값을 가두는 것 : clamp

            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);


        }
    }

    //맵을 이동할 때 바운드를 그 맵에 해당하는 바운드로 바꿔줘야 카메라 전환가능
    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }
}
