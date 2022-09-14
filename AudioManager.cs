using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //커스텀 클래스는 값을 지정하는 칸이 인스펙터창에 뜨지 않기때문에 강제로 띄워야함
public class Sound //커스텀 클래스 생성 
{
    public string name; //사운드의 이름

    public AudioClip clip; //사운드 파일
    private AudioSource source; //사운드 플레이어

    public float Volum;
    public bool loop;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.volume = Volum; //초기값 설정하게 만들어줌
    }
    
    public void SetVolumn()
    {
        source.volume = Volum;
    }

    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void SetLoop()
    {
        source.loop = true;
    }

    public void SetLoopCancel()
    {
        source.loop = false;
    }
}

public class AudioManager : MonoBehaviour
{
    //씬 이동할떄 AudioManager가 파괴되는 것을 방지
    static public AudioManager instance;

    [SerializeField]
    public Sound[] sounds;

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
        for(int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject("사운드 파일 이름 : " + i + " = " + sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());

            //사운드의 객체의 개수가 많으면 많을수록 하이어라키를 많이 차지하기때문에 SetParent기능 이용
            soundObject.transform.SetParent(this.transform);
            

        }
    }

    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if(_name == sounds[i].name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void SetLoop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoop();
                return;
            }
        }
    }

    public void SetLoopCancel(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoopCancel();
                return;
            }
        }
    }

    public void SetVolumn(string _name, float _Volumn)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Volum = _Volumn;
                sounds[i].SetVolumn();
                return;
            }
        }
    }
}
