using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class AutoFade : MonoBehaviour
{
    public TextMeshProUGUI _text;
    public float sec;

    private Coroutine _coroutine;
    private void OnEnable()
    {
        _text.DOFade(1, 0.01f);
        _coroutine = StartCoroutine(aotuFade());

    }

    IEnumerator aotuFade()
    {
        yield return new WaitForSeconds(sec);
        _text.DOFade(0, sec-1).onComplete += () =>
        {
            gameObject.SetActive(false);
        }; 
           
    }

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }
}
