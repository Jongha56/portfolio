



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MovingObject
{
    public float attackDelay; //공격유예시간, 플레이어가 슬라임의 공격을 피할수 있게 설정

    public float inter_MoveWaitTime; //대기 시간 -> 인스펙터창에서 설정
    private float current_interMWT; //실질적인 대기시간 계산은 여기서 설정

    public string atkSound;

    private Vector2 PlayerPos; //플레이어의 좌표값

    private int random_int; //npc가 랜덤으로 움직이기위한 변수
    private string direction; //MovingObject를 상속받아서 사용가능 -> base.Move() 에서 사용할 방향

    public GameObject healthBar; //flip함수때문에 몬스터의 체력바도 반대로 뒤집히기 때문에 수정해야함

    private string Boss_dir; //보스몹의 보고잇는방향

    // Start is called before the first frame update
    void Start()
    {
        queue = new Queue<string>(); //MovingObject는 Queue를 통해서 움직이기 때문에 생성해줘야함
        current_interMWT = inter_MoveWaitTime;

    }

    // Update is called once per frame
    void Update()
    {
        current_interMWT -= Time.deltaTime; //1초당 1씩 감소

        if (current_interMWT <= 0)
        {
            current_interMWT = inter_MoveWaitTime; //한번 작업이 끝나면 다시 초기화시켜서 반복할수있게
            BossPossition();
            if (NearPlayer()) //근처에 플레이어가 있을 경우 Flip 실행시키고 종료
            {
                Attacking(Boss_dir);
                return;
            }

            RandomDirection();

            //CheckCollison() 은 Vector값을 이용해서 어디로 이동하려 하는지를 판단, 비교하기 때문에
            //RandomDirection() 에서 그 Vector값을 초기화하고 다시 넣어줌
            if (base.CheckCollsion()) //앞에 장애물이 있을경우
            {
                queue.Clear();
                return; //그냥 종료
            }
            base.Move(direction);
        }
    }

    private void Attacking(string _direction) //슬라임 공격 에니메이션을 왼쪽 방향만 만들었기 때문에 scale을 -1로 바꿔서 오른쪽 공격모션을 취할수 있게 바꿔줌
    {
        switch (_direction)
        {
            case "UP":
                animator.SetTrigger("AttackUp"); //trigger기때문에 true, false값이 필요없이 실행가능
                break;
            case "DOWN":
                animator.SetTrigger("AttackDown");
                break;
            case "RIGHT":
                animator.SetTrigger("AttackRight");
                break;
            case "LEFT":
                animator.SetTrigger("AttackLeft");
                break;
        }
        StartCoroutine(WaitCoroutine());
    }
    private void BossPossition()
    {
        PlayerPos = PlayerManager.instance.transform.position;

        if (PlayerPos.x > this.transform.position.x)
            Boss_dir = "RIGHT";
        else if (PlayerPos.x < this.transform.position.x)
            Boss_dir = "LEFT";

        if (PlayerPos.y > this.transform.position.y)
            Boss_dir = "UP";
        else if (PlayerPos.y < this.transform.position.y)
            Boss_dir = "DOWN";
        
    }

    IEnumerator WaitCoroutine() //슬라임이 바로 공격하는것이 아니라 공격모션을 취하고 공격하기 때문에 살짝 대기해줘야함
    {
        yield return new WaitForSeconds(attackDelay);
        AudioManager.instance.Play(atkSound);
        if (NearPlayer()) //슬라임이 공격을 한 후 플레이어가 근처에 있으면 데미지를 입고, 근처에없으면 데미지를 입지않음
        {
            PlayerStat.instance.Hit(GetComponent<EnemyStat>().atk);
            //Debug.Log("슬라임이 플레이어에게 " + atk + "만큼의 데미지를 입혔습니다.");
        }

    }

    private bool NearPlayer() //몬스터가 공격을하려면 플레이어가 근처에 있어야함, 그것을 확인할 함수
    {
        PlayerPos = PlayerManager.instance.transform.position;

        //player와 slime의 위치값을 뺏을때 48pixel값보다 이하일경우 = 근처에있을경우
        //x좌표로만 비교하기때문에 슬라임 x좌표가 더 클경우 근처에 있지 않아도 조건을 만족하기 때문에 한번더 절댓값을 취해줌
        if (Mathf.Abs(PlayerPos.x - this.transform.position.x) <= speed * walkCount * 1.01f)
        {//비교값에 조금 여유를 둬서 미세한 오차범위 허용
            if (Mathf.Abs(PlayerPos.y - this.transform.position.y) <= speed * walkCount * 0.5)
            {//x좌표로만 1칸 떨어지고, y좌표 같을경우
                return true; // true 반환해서 플레이어가 근처에 있다는 것을 알려줌
            }
        }
        if (Mathf.Abs(PlayerPos.y - this.transform.position.y) <= speed * walkCount * 1.01f)
        {
            if (Mathf.Abs(PlayerPos.x - this.transform.position.x) <= speed * walkCount * 0.5)
            {//x좌표 같고, y좌표로만 1칸 떨어진 경우
                return true;
            }
        }
        return false; //플레이어가 근처에 없음

    }
    private void RandomDirection()
    {
        vector.Set(0, 0, vector.z);
        random_int = Random.Range(0, 4); //0~3
        switch (random_int)
        {
            case 0:
                vector.y = 1f;
                direction = "UP";
                break;
            case 1:
                vector.y = -1f;
                direction = "DOWN";
                break;
            case 2:
                vector.x = 1f;
                direction = "RIGHT";
                break;
            case 3:
                vector.x = -1f;
                direction = "LEFT";
                break;
        }
    }
}
