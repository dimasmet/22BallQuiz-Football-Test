using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ViewDataQuiz
{
    public int number;
    public Sprite imageQuiz;
    public string nameQuiz;
    public string difficulty;
    public string hashtag;
}

public class ScreenHome : MonoBehaviour
{
    public enum TypeQuiz
    {
        Football,
        Championship,
        Players
    }

    [SerializeField] private ButtonQuiz _prefabButtonQuiz;
    private List<ButtonQuiz> _buttonQuizzes;

    [SerializeField] private ViewDataQuiz[] _viewDataQuizzes;

    [SerializeField] private Transform _containerFootballQuiz;
    [SerializeField] private Transform _containerChampionshipQuiz;
    [SerializeField] private Transform _containerPlayersQuiz;

    [SerializeField] private ButtonType _typeFootballBtn;
    [SerializeField] private ButtonType _typeChampionshipBtn;
    [SerializeField] private ButtonType _typePlayersBtn;

    [SerializeField] private Button _playBtn;
    [SerializeField] private Button _settingsBtn;

    [SerializeField] private Transform _infoBlock;

    [Header("theme")]
    [SerializeField] private Text _title;
    [SerializeField] private Text _title2;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Color _darkColor;

    private void Awake()
    {
        _typeFootballBtn.SetButton(this, TypeQuiz.Football);
        _typeChampionshipBtn.SetButton(this, TypeQuiz.Championship);
        _typePlayersBtn.SetButton(this, TypeQuiz.Players);

        _playBtn.onClick.AddListener(() =>
        {
            AppHandler.Instance.SetQuestion(4);
            ScreensHandler.OpenScreen(ScreensHandler.ScreenName.QuestionScreen);
        });

        _settingsBtn.onClick.AddListener(() =>
        {
            ScreensHandler.OpenScreen(ScreensHandler.ScreenName.Settings);
        });
    }

    public Sprite GetImageQuiz(int num)
    {
        return _viewDataQuizzes[num].imageQuiz;
    }

    private void ChangeMode(ScreenSettings.DarkMode mode)
    {
        switch (mode)
        {
            case ScreenSettings.DarkMode.None:
                _settingsBtn.GetComponent<Image>().color = Color.black;
                _title.color = Color.black;
                _title2.color = Color.black;
                _backgroundImage.color = Color.white;
                break;
            case ScreenSettings.DarkMode.Dark:
                _settingsBtn.GetComponent<Image>().color = Color.white;
                _title.color = Color.white;
                _title2.color = Color.white;
                _backgroundImage.color = _darkColor;
                break;
        }

        for (int i = 0; i < _buttonQuizzes.Count; i++)
        {
            _buttonQuizzes[i].ChangeModeTheme(mode);
        }

        _typeFootballBtn.ChangeModeTheme(mode);
        _typeChampionshipBtn.ChangeModeTheme(mode);
        _typePlayersBtn.ChangeModeTheme(mode);
    }

    public void StartInit(QuizUserData quizUserData)
    {
        Transform container;

        _buttonQuizzes = new List<ButtonQuiz>();

        for (int i = 0; i < _viewDataQuizzes.Length; i++)
        {
            if (i < 4)
                container = _containerChampionshipQuiz.GetChild(0).GetChild(0);
            else
            {
                if (i >= 4 && i < 8)
                    container = _containerFootballQuiz.GetChild(0).GetChild(0);
                else
                {
                    container = _containerPlayersQuiz.GetChild(0).GetChild(0);
                }
            }

            ButtonQuiz buttonQuiz = Instantiate(_prefabButtonQuiz, container);
            buttonQuiz.gameObject.SetActive(true);

            buttonQuiz.InitButton(_viewDataQuizzes[i], quizUserData.quizUserResults[i]);

            _buttonQuizzes.Add(buttonQuiz);

            /*if (i == 4 || i == _viewDataQuizzes.Length - 1)
            {
                container.GetChild(0).transform.SetAsLastSibling();
            }*/
        }

        ChoiceType(TypeQuiz.Football);

        ScreenSettings.OnChangeMode += ChangeMode;
    }

    private void OnDestroy()
    {
        ScreenSettings.OnChangeMode -= ChangeMode;
    }

    public void ChoiceType(TypeQuiz type)
    {
        switch (type)
        {
            case TypeQuiz.Football:
                _containerFootballQuiz.gameObject.SetActive(true);
                Vector3 pos = _containerFootballQuiz.GetChild(0).GetChild(0).position;
                _containerFootballQuiz.GetChild(0).GetChild(0).position = new Vector3(pos.x, 0, pos.z);
                _infoBlock.transform.SetParent(_containerFootballQuiz.GetChild(0).GetChild(0));
                _containerChampionshipQuiz.gameObject.SetActive(false);
                _containerPlayersQuiz.gameObject.SetActive(false);

                _typeFootballBtn.ActiveType(true);
                _typeChampionshipBtn.ActiveType(false);
                _typePlayersBtn.ActiveType(false);
                break;
            case TypeQuiz.Championship:
                _containerFootballQuiz.gameObject.SetActive(false);
                _containerChampionshipQuiz.gameObject.SetActive(true);
                Vector3 pos2 = _containerFootballQuiz.GetChild(0).GetChild(0).position;
                _containerChampionshipQuiz.GetChild(0).GetChild(0).position = new Vector3(pos2.x, 0, pos2.z);
                _infoBlock.transform.SetParent(_containerChampionshipQuiz.GetChild(0).GetChild(0));
                _containerPlayersQuiz.gameObject.SetActive(false);

                _typeFootballBtn.ActiveType(false);
                _typeChampionshipBtn.ActiveType(true);
                _typePlayersBtn.ActiveType(false);
                break;
            case TypeQuiz.Players:
                _containerFootballQuiz.gameObject.SetActive(false);
                _containerChampionshipQuiz.gameObject.SetActive(false);
                _containerPlayersQuiz.gameObject.SetActive(true);
                Vector3 pos3 = _containerFootballQuiz.GetChild(0).GetChild(0).position;
                _containerPlayersQuiz.GetChild(0).GetChild(0).position = new Vector3(pos3.x, 0, pos3.z);
                _infoBlock.transform.SetParent(_containerPlayersQuiz.GetChild(0).GetChild(0));

                _typeFootballBtn.ActiveType(false);
                _typeChampionshipBtn.ActiveType(false);
                _typePlayersBtn.ActiveType(true);
                break;
        }
    }
}
