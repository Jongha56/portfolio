using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    static public WeatherManager instance; //싱글턴화

    private void Awake()
    {
        //캐릭터가 맵을 이동할때마다 자신이 복제가 되기 때문에 조건문을 달아서 
        //처음맵을 이동할때만 파괴시키지 않고 그 뒤부터는 파괴시키게 만듦
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject); //다른 씬으로 갈 때마다 게임오브젝트 파괴방지 
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private AudioManager theAudio;
    public ParticleSystem rain;
    public string rain_sound;
    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();   
    }

    public void Rain()
    {
        theAudio.Play(rain_sound);
        rain.Play();
    }
    public void RainStop()
    {
        theAudio.Stop(rain_sound);
        rain.Stop();
    }
}