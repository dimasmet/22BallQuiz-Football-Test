using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

        GetQuizFromFile("QuizPlayers1", quizsPlayers);
        GetQuizFromFile("QuizPlayers2", quizsPlayers);
        GetQuizFromFile("QuizPlayers3", quizsPlayers);
        GetQuizFromFile("QuizPlayers4", quizsPlayers);
    }

    [SerializeField] private List<Quiz> quizsFootballClub;
    [SerializeField] private List<Quiz> quizsChampionship;
    [SerializeField] private List<Quiz> quizsPlayers;

    [SerializeField] private ScreenQuestion _screenQuestion;
    [SerializeField] private ScreenResult _screenResult;
    [SerializeField] private ScreenHome _screenHome;

    [SerializeField] private QuizUserData quizUserData;

    private Quiz currentQuiz;
    private int numberQuestion;
    private bool isNext = false;

    private int numberQuiz;

    private int score = 0;

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

    private void OnDestroy()
    {
        TimerQuiz.OnEndTimer -= TimerEnd;
    }

    public void SetQuestion(int numberQuiz)
    {
        this.numberQuiz = numberQuiz;

        if (numberQuiz < 4)
            currentQuiz = quizsFootballClub[numberQuiz];
        else
        {
            if (numberQuiz >= 4 && numberQuiz < 8)
            {
                int number = numberQuiz - 4;
                currentQuiz = quizsFootballClub[number];
            }
            else
            {
                int number = numberQuiz - 8;
                currentQuiz = quizsPlayers[number];
            }
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
