using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class QuizWrapper
{
    public Quiz quiz;
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

    private Quiz currentQuiz;
    private int numberQuestion;
    private bool isNext = false;

    private int numberQuiz;

    private int score = 0;

    private void Start()
    {
        TimerQuiz.OnEndTimer += TimerEnd;
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
            int number = numberQuiz - 4;
            currentQuiz = quizsFootballClub[number];
        }
        score = 0;
        numberQuestion = -1;
        isNext = true;

        TimerQuiz.OnStartTimer?.Invoke();
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
                TimerQuiz.OnStopTimer?.Invoke();
            }
        }
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
