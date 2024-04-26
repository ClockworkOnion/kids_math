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
    private int scoreStreak = 0;

    private List<int[]> history = new List<int[]>();

    public StageSummary stageSummary;
    private TextMeshProUGUI scoreText;

    private GameObject historyPanel;
    private TextMeshProUGUI historyText;

    private bool showHistory = false;

    private void Awake()
    {
        //stageSummary = GameObject.Find("StageSummaryCanvas"); // assign in inspector
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();

        historyPanel = GameObject.Find("History");
        historyText = historyPanel.GetComponentInChildren<TextMeshProUGUI>();
        historyPanel.SetActive(false);
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

    /*
    SCORING SYSTEM
    addLv1 -> subLv1 -> AddSubLv1 -> (5 point)
    Addlv2 -> subLv2 -> AddSubLv2 -> (10 point)
    Addlv3 -> subLv3 -> AddSubLv3 -> (20 point)
    Addlv4 -> subLv4 -> AddSubLv4 -> (40 point)

    MultLv1 -> DivLv1 -> MultDivLv1 -> (20 point)
    MultLv2 -> DivLv2 -> MultDivLv2 -> (40 point)
    MultLv3 -> DivLv3 -> MultDivLv3 -> (80 point)
    MultLv4 -> DivLv4 -> MultDivLv4 -> (120 point)

    AddSubMultDivLv1 -> ... AddSubMultDivLv4 (200 point)

    3x right answer -> double points
    5x right answer -> triple points
    */

    /// <summary>
    /// Records the whether an equation was solved correctly or not and returns the appropriate score
    /// </summary>
    /// <param name="correctly">True if the equation was solved correctly, false if not</param>
    /// <returns>The amount of score awarded for solving the equation, or 0 if it was solved incorrectly</returns>
    public int ProblemDone(bool correctly, StageManager.GameMode currentGameMode)
    {
        problemsTotal++;
        if (correctly)
        {
            problemsSolved++;
            scoreStreak++;
        }
        else
        {
            scoreStreak = 0;
            return 0;
        }

        int baseScore = scores[currentGameMode];
        int multiplier = scoreStreak > 5 ? 3 : scoreStreak > 3 ? 2 : 1;
        return baseScore * multiplier;
    }

    private Dictionary<StageManager.GameMode, int> scores = new Dictionary<StageManager.GameMode, int> {
            {StageManager.GameMode.addLv1, 5 },
            {StageManager.GameMode.subLv1, 5 },
            {StageManager.GameMode.addSubLv1, 5 },

            {StageManager.GameMode.addLv2, 10 },
            {StageManager.GameMode.subLv2, 10 },
            {StageManager.GameMode.addSubLv2, 10},

            {StageManager.GameMode.addLv3, 20},
            {StageManager.GameMode.subLv3, 20},
            {StageManager.GameMode.addSubLv3, 20},

            {StageManager.GameMode.addLv4, 40},
            {StageManager.GameMode.subLv4, 40},
            {StageManager.GameMode.addSubLv4, 40},

            {StageManager.GameMode.multLv1, 20},
            {StageManager.GameMode.divLv1, 20},
            {StageManager.GameMode.multDivLv1, 20},

            {StageManager.GameMode.multLv2, 40},
            {StageManager.GameMode.divLv2, 40},
            {StageManager.GameMode.multDivLv2, 40},

            {StageManager.GameMode.multLv3, 80},
            {StageManager.GameMode.divLv3, 80},
            {StageManager.GameMode.multDivLv3, 80},

            {StageManager.GameMode.multLv4, 120},
            {StageManager.GameMode.divLv4, 120},
            {StageManager.GameMode.multDivLv4, 120},

            {StageManager.GameMode.addSubMultDivLv1, 30},
            {StageManager.GameMode.addSubMultDivLv2, 60},
            {StageManager.GameMode.addSubMultDivLv3, 120},
            {StageManager.GameMode.addSubMultDivLv4, 180},
        };

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

    public float GetRatio()
    {
        return (problemsTotal > 0) ? ((float)problemsSolved / (float)problemsTotal) * 100 : 100;
    }

    public void ShowScores(int coinsEarned)
    {
        stageSummary.Show();
        stageSummary.ShowScores(problemsSolved, problemsTotal - problemsSolved, coinsEarned);

        /*
        float ratio = GetRatio();

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
        */
    }

    public void HideScores()
    {
        stageSummary.Hide();
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
