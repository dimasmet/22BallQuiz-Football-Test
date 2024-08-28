using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonType : MonoBehaviour
{
    [SerializeField] private Button _thisButton;
    [SerializeField] private Image _image;

    [SerializeField] private Sprite spriteActive;
    [SerializeField] private Sprite spriteNoActive;

    private ScreenHome _screenHome;
    private ScreenHome.TypeQuiz typeQuiz;

    [SerializeField] private Text _text;

    public void ChangeModeTheme(ScreenSettings.DarkMode mode)
    {
        switch (mode)
        {
            case ScreenSettings.DarkMode.None:
                _text.color = Color.black;
                break;
            case ScreenSettings.DarkMode.Dark:
                _text.color = Color.white;
                break;
        }
    }

    public void SetButton(ScreenHome screenHome, ScreenHome.TypeQuiz typeQuiz)
    {
        _screenHome = screenHome;
        this.typeQuiz = typeQuiz;

        _thisButton.onClick.AddListener(() =>
        {
            screenHome.ChoiceType(typeQuiz);
        });
    }

    public void ActiveType(bool isActive)
    {
        if (isActive)
            _image.sprite = spriteActive;
        else
            _image.sprite = spriteNoActive;
    }
}
