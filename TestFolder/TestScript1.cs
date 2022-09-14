using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript1 : MonoBehaviour
{
    BGMManager BGM;

    public int playMusicTrack;
    
    // Start is called before the first frame update
    void Start()
    {
        BGM = FindObjectOfType<BGMManager>();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BGM.Play(playMusicTrack);
        this.gameObject.SetActive(false); //실행 후 하이어라키에서 제외시키는 함수 움직엿다 다시가면 처음부터 다시 bgm이 시작되는 것을 방지
    }
}
