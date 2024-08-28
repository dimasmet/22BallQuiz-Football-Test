using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Answer
{
    public string textAnswer;
}

[Serializable]
public class Question
{
    public string textQuestion;
    public int numberTrueAnswer;
    public List<Answer> answers;
}

[Serializable]
public class Quiz
{
    public int number;
    public List<Question> questions;
}

