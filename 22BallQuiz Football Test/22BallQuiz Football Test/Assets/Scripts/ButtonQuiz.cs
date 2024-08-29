using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonQuiz : MonoBehaviour
{
    public static Action OnChangeResultMaxUser;

    [SerializeField] private Button _thisButton;

    [SerializeField] private Image _ImageQuiz;
    [SerializeField] private Text _nameQuizText;
    [SerializeField] private Text _difficultyText;
    [SerializeField] private Text _hashtagText;

    [SerializeField] private Text _maxResultUserText;

    private int numberQuiz;

    [Header("Theme")]
    [SerializeField] private Image _imgBackground;
    [SerializeField] private Color _colorDark;

    private QuizUserResult quizUser;

    private void Awake()
    {
        _thisButton.onClick.AddListener(() =>
        {
            AppHandler.Instance.SetQuestion(numberQuiz);

            ScreensHandler.OpenScreen?.Invoke(ScreensHandler.ScreenName.QuestionScreen);
        });
    }

    public void ChangeModeTheme(ScreenSettings.DarkMode mode)
    {
        switch (mode)
        {
            case ScreenSettings.DarkMode.None:
                _imgBackground.color = Color.white;
                _nameQuizText.color = Color.black;
                break;
            case ScreenSettings.DarkMode.Dark:
                _imgBackground.color = _colorDark;
                _nameQuizText.color = Color.white;
                break;
        }
    }

    public void InitButton(ViewDataQuiz viewData, QuizUserResult quizUser)
    {
        this.quizUser = quizUser;
        numberQuiz = viewData.number;

        _maxResultUserText.text = "Max result " + this.quizUser.resultMax + "/10";
        _ImageQuiz.sprite = viewData.imageQuiz;
        _nameQuizText.text = viewData.nameQuiz;
        _difficultyText.text = viewData.difficulty;
        _hashtagText.text = viewData.hashtag;
    }

    private void Start()
    {
        OnChangeResultMaxUser += UpdateInfoQuiz;
    }

    private void OnDestroy()
    {
        OnChangeResultMaxUser -= UpdateInfoQuiz;
    }

    private void UpdateInfoQuiz()
    {
        _maxResultUserText.text = "Max result " + this.quizUser.resultMax + "/10";
    }
}
