using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreTracker : MonoBehaviour
{
    private int problemsTotal = 0;
    private int problemsSolved = 0;
    private float topSpeed = 0;

    private List<int[]> history = new List<int[]>();

    private GameObject scoreCanvas;
    private TextMeshProUGUI scoreText;

    private GameObject historyPanel;
    private TextMeshProUGUI historyText;

    private bool showHistory = false;

    private void Awake()
    {
        scoreCanvas = GameObject.Find("StageSummaryCanvas");
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();

        historyPanel = GameObject.Find("History");
        historyText = historyPanel.GetComponentInChildren<TextMeshProUGUI>();
        historyPanel.SetActive(false);
        scoreCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            showHistory = !showHistory;
            if (showHistory)
            {
                historyPanel.SetActive(true);
                UpdateHistoryPanel();
            }
            else
            {
                historyPanel.SetActive(false);
            }
        }
    }

    public void ResetAll()
    {
        history.Add(new int[2] { problemsTotal, problemsSolved });
        problemsSolved = 0;
        problemsTotal = 0;
        topSpeed = 0f;
    }

    public void ProblemDone(bool correctly)
    {
        problemsTotal++;
        if (correctly) problemsSolved++;
    }

    public void UpdateTopSpeed(float speed)
    {
        topSpeed = (speed > topSpeed) ? speed : topSpeed;
    }

    public int GetTotal()
    {
        return problemsTotal;
    }

    public int GetSolved()
    {
        return problemsSolved;
    }

    public float GetTopSpeed()
    {
        return topSpeed;
    }

    public void ShowScores()
    {
        scoreCanvas.SetActive(true);
        float ratio = (problemsTotal > 0) ? ((float)problemsSolved / (float)problemsTotal) * 100 : 100;

        string solvedCounter = "";
        for (int i = 0; i < problemsSolved; i++)
        {
            solvedCounter += "O";
        }

        string missedCounter = "";
        for (int i = 0; i < problemsTotal - problemsSolved; i++)
        {
            missedCounter += "X";
        }

        string message = (ratio > 99) ? "Perfect! Now try a harder mode?"
        : (ratio > 80) ? "You did great!"
        : (ratio > 50) ? "You solved most problems correctly!"
        : (ratio > 30) ? "Try again? You can do it!"
        : "Keep on practicing!";
        scoreText.text = $"Right answers:\n{solvedCounter}\nWrong answers:\n{missedCounter}\n{message}\nPress Space to Continue!";
        // TODO add a pop in animation
    }

    public void HideScores()
    {
        scoreCanvas.SetActive(false);
        // TODO add a fade out animation
    }

    public void UpdateHistoryPanel()
    {
        history.ForEach((x) => Debug.Log("Total: " + x[0] + ", solved: " + x[1]));
        int numberOfRuns = history.Count;
        int sumHistoryTotal = 0;
        int sumHistoryCorrect = 0;
        history.ForEach((x) =>
        {
            sumHistoryTotal += x[0];
            sumHistoryCorrect += x[1];
        });
        historyText.text = $"History:\nRuns: {numberOfRuns}\nTotal {sumHistoryCorrect} correct\nout of {sumHistoryTotal} problems.";
    }
}
