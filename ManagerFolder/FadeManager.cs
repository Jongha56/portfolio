using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public SpriteRenderer white;
    public SpriteRenderer black;
    private Color color;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    public void FadeOut(float _speed = 0.02f)  //화면 어두워지는 효과
    {
        StopAllCoroutines(); //맵왓다갓다이동할떄 코루팅 중첩방지
        StartCoroutine(FadeOutCoroutine(_speed));
    }
    IEnumerator FadeOutCoroutine(float _speed)
    {
        color = black.color;

        while (color.a < 1f)
        {
            color.a += _speed;
            black.color = color;
            yield return waitTime;
        }
    }

    public void FadeIn(float _speed = 0.02f) //화면 밝아지는 효과
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(_speed));
    }
    IEnumerator FadeInCoroutine(float _speed)
    {
        color = black.color;

        while (color.a > 0f)
        {
            color.a -= _speed;
            black.color = color;
            yield return waitTime;
        }
    }

    public void Flash(float _speed = 0.1f) //잠깐 화면 빛나는 효과
    {
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine(_speed));
    }
    IEnumerator FlashCoroutine(float _speed)
    {
        color = white.color;

        while (color.a < 1f)
        {
            color.a += _speed;
            white.color = color;
            yield return waitTime;
        }
        while (color.a > 0f)
        {
            color.a -= _speed;
            white.color = color;
            yield return waitTime;
        }
    }

    public void FlashOut(float _speed = 0.02f) 
    {
        StopAllCoroutines();
        StartCoroutine(FlashOutCoroutine(_speed));
    }
    IEnumerator FlashOutCoroutine(float _speed)
    {
        color = white.color;

        while (color.a < 1f)
        {
            color.a += _speed;
            white.color = color;
            yield return waitTime;
        }
    }
    public void FlashIn(float _speed = 0.02f) 
    {
        StopAllCoroutines();
        StartCoroutine(FlashInCoroutine(_speed));
    }
    IEnumerator FlashInCoroutine(float _speed)
    {
        color = white.color;

        while (color.a > 0f)
        {
            color.a -= _speed;
            white.color = color;
            yield return waitTime;
        }
    }
}