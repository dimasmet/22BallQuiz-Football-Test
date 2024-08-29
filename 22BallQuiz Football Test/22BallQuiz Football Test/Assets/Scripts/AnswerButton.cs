using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public enum State
    {
        None,
        True,
        False
    }

    [SerializeField] private int _numberAnswer;
    [SerializeField] private Button _thisButton;
    [SerializeField] private Text _textAnswer;

    [Header("Image button answer")]
    [SerializeField] private Image _sourceImage;

    [Header("Sprites to button")]
    [SerializeField] private Sprite _sourceSprite;
    [SerializeField] private Sprite _trueSprite;
    [SerializeField] private Sprite _falseSprite;

    [Header("Sprites to dark mode")]
    [SerializeField] private Sprite _sourceSpriteWhite;
    [SerializeField] private Sprite _falseSpriteWhite;
    [SerializeField] private Sprite _sourceSpriteDark;
    [SerializeField] private Sprite _falseSpriteDark;

    private State currentState;

    public static bool isActive = false;

    private void Awake()
    {
        _thisButton.onClick.AddListener(() =>
        {
            if (isActive)
            {
                AppHandler.Instance.ChoiceAnswerButton(_numberAnswer, this);
                isActive = false;
            }
        });
    }

    public void ChangeMode(ScreenSettings.DarkMode mode)
    {
        switch (mode)
        {
            case ScreenSettings.DarkMode.None:
                _sourceSprite = _sourceSpriteWhite;
                _falseSprite = _falseSpriteWhite;
                _textAnswer.color = Color.black;
                break;
            case ScreenSettings.DarkMode.Dark:
                _sourceSprite = _sourceSpriteDark;
                _falseSprite = _falseSpriteDark;
                _textAnswer.color = Color.white;
                break;
        }

        _sourceImage.sprite = _sourceSprite;
    }

    public void SetTextAnswer(string answerString)
    {
        _textAnswer.text = answerString;
        isActive = true;
        ChangeState(State.None);
    }

    public void ChangeState(State state)
    {
        currentState = state;

        switch (currentState)
        {
            case State.None:
                _sourceImage.sprite = _sourceSprite;
                break;
            case State.True:
                _sourceImage.sprite = _trueSprite;
                break;
            case State.False:
                _sourceImage.sprite = _falseSprite;
                break;
        }
    }
}
