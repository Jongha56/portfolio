using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance; //awake함수와 함께 싱글턴화 해서 파괴되지 않게 만들어줌

    public AudioClip[] clips; //배경음악들

    private AudioSource source;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

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
        source = GetComponent<AudioSource>();
    }

    public void Play(int _playMusicTrack)
    {
        source.volume = 1f; //fadeout후 소리가 0이되서 안들릴 경우 다시 키워서 소리를 들리게 만들어줘야함
        source.clip = clips[_playMusicTrack];
        source.Play();
    }

    public void SetVolumn(float _volumn)
    {
        source.volume = _volumn;
    }

    public void Pause()
    {
        source.Pause();
    }

    public void Unpause()
    {
        source.UnPause();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void FadeOutMusic() //음악이 점점 작게 들리다가 종료
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }

    IEnumerator FadeOutMusicCoroutine()
    {
        for (float i=1.0f;i>0f;i-=0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }

    public void FadeInMusic() //음악이 점점 크게 들리게 만듦
    {
        StopAllCoroutines();
        StartCoroutine(FadeInMusicCoroutine());
    }

    IEnumerator FadeInMusicCoroutine()
    {
        for (float i = 0f; i > 1f; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }
}
