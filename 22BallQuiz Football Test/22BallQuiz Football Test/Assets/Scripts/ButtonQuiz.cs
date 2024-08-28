using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonQuiz : MonoBehaviour
{
    [SerializeField] private Button _thisButton;

    [SerializeField] private Image _ImageQuiz;
    [SerializeField] private Text _nameQuizText;
    [SerializeField] private Text _difficultyText;
    [SerializeField] private Text _hashtagText;

    private int numberQuiz;

    [Header("Theme")]
    [SerializeField] private Image _imgBackground;
    [SerializeField] private Color _colorDark;

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

    public void InitButton(ViewDataQuiz viewData)
    {
        numberQuiz = viewData.number;

        _ImageQuiz.sprite = viewData.imageQuiz;
        _nameQuizText.text = viewData.nameQuiz;
        _difficultyText.text = viewData.difficulty;
        _hashtagText.text = viewData.hashtag;
    }
}
