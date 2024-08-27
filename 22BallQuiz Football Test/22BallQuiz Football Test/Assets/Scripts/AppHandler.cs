using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppHandler : MonoBehaviour
{
    public static AppHandler Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] private AnswerButton[] _answerButtons;

    public void ChoiceAnswerButton(int number)
    {

    }
}
