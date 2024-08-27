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

    private State currentState;

    private void Awake()
    {
        _thisButton.onClick.AddListener(() =>
        {

        });
    }

    public void SetTextAnswer(string answerString)
    {
        _textAnswer.text = answerString;

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
