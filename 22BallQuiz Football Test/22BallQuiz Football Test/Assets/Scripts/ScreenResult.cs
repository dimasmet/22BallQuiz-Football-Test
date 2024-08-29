using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenResult : MonoBehaviour
{
    public enum Result
    {
        Success,
        TimeOut
    }

    [SerializeField] private GameObject _panel;

    [SerializeField] private GameObject _successGo;
    [SerializeField] private GameObject _falseGo;

    [SerializeField] private Button _homeBtn;
    [SerializeField] private Button _nextBtn;
    [SerializeField] private Button _restartBtn;

    [SerializeField] private Text _resultText;

    [Header("Theme")]
    [SerializeField] private Image _panelImage1;
    [SerializeField] private Image _panelImage2;
    [SerializeField] private Sprite _panelWhite;
    [SerializeField] private Sprite _panelDark;
    [SerializeField] private Text _title1;
    [SerializeField] private Text _title2;
    [SerializeField] private Text _title3;
    [SerializeField] private Text _title4;

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
                _panelImage1.sprite = _panelWhite;
                _panelImage2.sprite = _panelWhite;
                _title1.color = Color.black;
                _title2.color = Color.black;
                _title3.color = Color.black;
                _title4.color = Color.black;
                _resultText.color = Color.black;

                break;
            case ScreenSettings.DarkMode.Dark:
                _panelImage1.sprite = _panelDark;
                _panelImage2.sprite = _panelDark;
                _title1.color = Color.white;
                _title2.color = Color.white;
                _title3.color = Color.white;
                _title4.color = Color.white;
                _resultText.color = Color.white;

                break;
        }
    }

    private void Awake()
    {
        _homeBtn.onClick.AddListener(() =>
        {
            ScreensHandler.OpenScreen?.Invoke(ScreensHandler.ScreenName.HomeScreen);
            _panel.SetActive(false);
        });

        _nextBtn.onClick.AddListener(() =>
        {
            AppHandler.Instance.NextQuiz();
            _panel.SetActive(false);
        });

        _restartBtn.onClick.AddListener(() =>
        {
            AppHandler.Instance.TryQuiz();
            _panel.SetActive(false);
        });
    }

    public void ShowResult(Result result, int countScore = 0)
    {
        _panel.SetActive(true);

        switch (result)
        {
            case Result.Success:
                _successGo.SetActive(true);
                _falseGo.SetActive(false);
                _resultText.text = countScore + "/10";
                break;
            case Result.TimeOut:
                _successGo.SetActive(false);
                _falseGo.SetActive(true);
                _resultText.text = "";
                break;
        }
    }
}
