using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerQuiz : MonoBehaviour
{
    public static Action OnStartTimer;
    public static Action OnStopTimer;
    public static Action OnEndTimer;

    [SerializeField] private float time;
    [SerializeField] private Text timerText;

    private float _timeLeft = 0f;

    private void Start()
    {
        OnStartTimer += StartTimer;
        OnStopTimer += StopTimer;
    }

    private void OnDestroy()
    {
        OnStartTimer -= StartTimer;
        OnStopTimer -= StopTimer;
    }

    public void StartTimer()
    {
        _timeLeft = time;
        StartCoroutine(WaitTime());
    }

    private void StopTimer()
    {
        StopAllCoroutines();
    }

    private IEnumerator WaitTime()
    {
        while (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            UpdateTimeText();
            yield return null;
        }

        OnEndTimer?.Invoke();
    }

    private void UpdateTimeText()
    {
        if (_timeLeft < 0)
            _timeLeft = 0;

        timerText.text = string.Format("{0:00} : {1:00}", Mathf.FloorToInt(_timeLeft / 60), Mathf.FloorToInt(_timeLeft % 60));
    }
}
