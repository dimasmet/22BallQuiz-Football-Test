using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenQuestion : MonoBehaviour
{
    [SerializeField] private Button _nextAnswerBtn;
    [SerializeField] private Text _numberQuestionText;

    [SerializeField] private Text _questionText;
    [SerializeField] private AnswerButton[] _answerButtons;

    [SerializeField] private Button _backBtn;

    [Header("Theme")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _backgroundPanelQuestion;
    [SerializeField] private Sprite _whiteSpritePanelQuestion;
    [SerializeField] private Sprite _darkSpritePanelQuestion;
    [SerializeField] private Color _colorDark;

    private void Start()
    {
        ScreenSettings.OnChangeMode += ChangeMode;
    }

    private void OnDestroy()
    {
        ScreenSettings.OnChangeMode -= ChangeMode;
    }

    private void ChangeMode(ScreenSettings.DarkMode mode)
    {
        switch (mode)
        {
            case ScreenSettings.DarkMode.None:
                _backgroundImage.color = Color.white;
                _backgroundPanelQuestion.sprite = _whiteSpritePanelQuestion;
                _questionText.color = Color.black;
                _backBtn.GetComponent<Image>().color = _colorDark;
                break;
            case ScreenSettings.DarkMode.Dark:
                _backgroundImage.color = _colorDark;
                _backgroundPanelQuestion.sprite = _darkSpritePanelQuestion;
                _questionText.color = Color.white;
                _backBtn.GetComponent<Image>().color = Color.white;
                break;
        }

        for (int i = 0; i < _answerButtons.Length; i++)
        {
            _answerButtons[i].ChangeMode(mode);
        }
    }

    public void Show(Question question, int numberQuestion)
    {
         _numberQuestionText.text = (numberQuestion + 1).ToString();
         _questionText.text = question.textQuestion;

         for (int i = 0; i < _answerButtons.Length; i++)
         {
            _answerButtons[i].SetTextAnswer(question.answers[i].textAnswer);
         }
    }

    private void Awake()
    {
        _nextAnswerBtn.onClick.AddListener(() =>
        {
            AppHandler.Instance.NextQuestion();
        });

        _backBtn.onClick.AddListener(() =>
        {
            ScreensHandler.OpenScreen?.Invoke(ScreensHandler.ScreenName.HomeScreen);
            TimerQuiz.OnStopTimer?.Invoke();
        });
    }
}
