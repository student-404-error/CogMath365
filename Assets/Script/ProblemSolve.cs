using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class ProblemSolve : MonoBehaviour
{
    public TMP_Text problemText;
    public TMP_InputField inputField;
    public TMP_Text feedbackText;
    public TMP_Text scoreText;
    public Button submit;

    private int currentProblemResult;
    private int score = 0;

    private void Start()
    {
        GenerateProblem();
    }

    public void SubmitAnswer()
    {
        int userAnswer;
        if (int.TryParse(inputField.text, out userAnswer))
        {
            if (userAnswer == currentProblemResult)
            {
                feedbackText.text = "Correct!";
                score += 10;
            }
            else
            {
                feedbackText.text = "Wrong!";
            }

            scoreText.text = "Score: " + score.ToString();

            GenerateProblem();
        }
        else
        {
            feedbackText.text = "Invalid input";
        }

        inputField.text = "";
    }

    private void GenerateProblem()
    {
        int num1 = Random.Range(1, 101);
        int num2 = Random.Range(1, 101);
        bool isSubtraction = Random.Range(0, 2) == 0; // 50% chance of subtraction

        if (problemText != null)
        {
            if (isSubtraction)
            {
                currentProblemResult = num1 - num2;
                problemText.text = num1.ToString() + " - " + num2.ToString();
            }
            else
            {
                currentProblemResult = num1 + num2;
                problemText.text = num1.ToString() + " + " + num2.ToString();
            }
        }

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }

    }
}